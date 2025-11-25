using System.Diagnostics;
using System.Globalization;
using MobiMart.Model;
using MobiMart.ViewModel;
using Plugin.LocalNotification;
using SQLite;

namespace MobiMart.Service;




public class NotificationService
{
    static SQLiteAsyncConnection? db;
    string baseUrl = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5199" : "http://localhost:5198";
    HttpClient client;

    string[] formats = new[] {
        "M/d/yyyy h:mm:ss tt",
        "MM/dd/yyyy hh:mm:ss tt",
        "MM/dd/yyyy h:mm tt",
        "MM/dd/yyyy hh:mm tt",
        "MM/dd/yyyy"
    };

    public NotificationService()
    {
        if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            if (DeviceInfo.DeviceType == DeviceType.Virtual)
                baseUrl = "http://10.0.2.2:5199"; // emulator
            else
                baseUrl = "http://172.20.10.5:5199"; // physical device (replace with server [LAN] IP)
        }
        else
        {
            baseUrl = "http://localhost:5199"; // Windows
        }

        client = new HttpClient()
        {
            BaseAddress = new Uri(baseUrl)
        };
        client.Timeout = TimeSpan.FromSeconds(8);
    }


    async Task Init()
    {
        if (db != null) return;
        var databasePath = Path.Combine(FileSystem.AppDataDirectory, "mobimart.db");
        db = new SQLiteAsyncConnection(databasePath);

        await db.CreateTableAsync<Reminder>();
    }



    public async Task ScheduleLocalNotification(string title, string body, DateTime notifyAt, string payload = null)
    {
        var request = new NotificationRequest
        {
            Title = title,
            Description = body,
            ReturningData = payload, // pass small string data to read when tapped
            Schedule =
            {
                NotifyTime = notifyAt
            }
        };

        await LocalNotificationCenter.Current.Show(request);
    }



    public async Task AddReminderAsync(Reminder r)
    {
        await Init();
        r.LastUpdatedAt = DateTimeOffset.UtcNow;
        await db!.InsertAsync(r);
    }


    public async Task<List<Reminder>> GetAllRemindersAsync()
    {
        await Init();
        var businessId = Guid.Empty;
        if (Shell.Current.BindingContext is FlyoutMenuViewModel vm)
        {
            businessId = vm.BusinessId;
        }

        if (businessId == Guid.Empty) return [];
        return await db!.Table<Reminder>().Where(x=> x.BusinessId == businessId).ToListAsync();
    }


    public async Task DeleteReminderAsync(Reminder r)
    {
        await Init();
        // soft delete
        r.IsDeleted = true;
        r.LastUpdatedAt = DateTimeOffset.UtcNow;
        await db!.UpdateAsync(r);
        // await db!.DeleteAsync(r);
    }


    public async Task UpdateReminderAsync(Reminder r)
    {
        await Init();
        r.LastUpdatedAt = DateTimeOffset.UtcNow;
        await db!.UpdateAsync(r);
    }


    public async Task CheckAndScheduleNotificationsAsync(InventoryService inventoryService)
    {
        var reminders = new List<Reminder>();
        try
        {
            reminders = await GetAllRemindersAsync();
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.StackTrace);
        }

        Debug.WriteLine("-------->>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
        Debug.WriteLine(DateTime.Now.ToString());

        if (reminders is null) return;

        for (int i = 0; i < reminders.Count; i++)
        {
            var reminder = reminders[i];
            if (reminder.Type is ReminderType.ConsignmentDue)
            {
                // deletion if one week past the notify date already
                if (DateTimeOffset.UtcNow > reminder.NotifyAtDate.AddDays(7))
                {
                    await DeleteReminderAsync(reminder);
                    continue;
                }

                // update message
                var delivery = await inventoryService.GetDeliveryAsync(reminder.RelatedEntityId);
                // item has been returned already
                if (delivery.ReturnByDate is null || delivery.ReturnByDate.Equals(""))
                {
                    await DeleteReminderAsync(reminder);
                    continue;
                }
                var inv = await inventoryService.GetInventoryFromDeliveryAsync(delivery.Id);
                var item = await inventoryService.GetItemAsync(inv.ItemBarcode);

                var message = $"""
                The item {item.Name} delivered on {delivery.DateDelivered.LocalDateTime:MM/dd/yyyy} is to be returned on {delivery.ReturnByDate.Value.LocalDateTime:MM/dd/yyyy}.
                Items Sold: {delivery.DeliveryAmount - inv.TotalAmount}
                Stock Remaining: {inv.TotalAmount} / {delivery.DeliveryAmount}
                Amount to Pay: {(delivery.DeliveryAmount - inv.TotalAmount) * (delivery.BatchWorth / delivery.DeliveryAmount):0.00}
                """;

                reminder.Message = message;
                await UpdateReminderAsync(reminder);
            }
            else if (reminder.Type is ReminderType.SupplyRunout)
            {
                // check if restocked or not already
                var inv = await inventoryService.GetInventoryAsync(reminder.RelatedEntityId);
                if (inv is null || inv.IsDeleted) // inv record does not exist anymore (meaning stock has been depleted)
                {
                    await DeleteReminderAsync(reminder);
                    continue;
                }
                
                // all the inventory records of the item
                var invRecords = await inventoryService.GetInventoriesAsync(inv.ItemBarcode);
                if (invRecords is null || invRecords.Count <= 0) continue; // same same validation sa taas
                int totalInv = 0;
                foreach (var _i in invRecords) totalInv += _i.TotalAmount;
                
                // delete reminder if there is enough stock already
                if (totalInv >= 10)
                {
                    await DeleteReminderAsync(reminder);
                    continue;
                }

                // else, resched reminder and local notification
                var item = await inventoryService.GetItemAsync(inv.ItemBarcode);
                var message = $"Stock for item {item.Name} is running low\nRemaining Stock: {totalInv}";
                reminder.Message = message;
                await UpdateReminderAsync(reminder);

                if (reminder.NotifyAtDate.Date < DateTimeOffset.UtcNow.Date) // only when today's date (by day) is > previous notifydate sched
                {
                    reminder.NotifyAtDate = DateTimeOffset.UtcNow.AddMinutes(2);
                    await ScheduleLocalNotification(reminder.Title, reminder.Message, reminder.NotifyAtDate.LocalDateTime, reminder.Id.ToString());
                }
            }
            else if (reminder.Type is ReminderType.DeliveryReminder)
            {
                if (reminder.NotifyAtDate < DateTimeOffset.UtcNow)
                {
                    await DeleteReminderAsync(reminder);
                }
            }
        }
    }
}
