using System;
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



    public async Task ScheduleLocalNotification(int id, string title, string body, DateTime notifyAt, string payload = null)
    {
        var request = new NotificationRequest
        {
            NotificationId = id,
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
        await db!.InsertAsync(r);
    }


    public async Task<List<Reminder>> GetAllRemindersAsync()
    {
        await Init();
        var businessId = -1;
        if (Shell.Current.BindingContext is FlyoutMenuViewModel vm)
        {
            businessId = vm.BusinessId;
        }
        return await db!.Table<Reminder>().Where(x => x.BusinessId == businessId).ToListAsync();
    }


    public async Task DeleteReminderAsync(Reminder r)
    {
        await Init();
        await db!.DeleteAsync(r);
    }


    public async Task UpdateReminderAsync(Reminder r)
    {
        await Init();
        await db!.UpdateAsync(r);
    }


    public async Task CheckAndScheduleNotificationsAsync(InventoryService inventoryService)
    {
        var reminders = await GetAllRemindersAsync();

        for (int i = 0; i < reminders.Count; i++)
        {
            var reminder = reminders[i];

            if (reminder.Type is ReminderType.ConsignmentDue)
            {
                // deletion if one week past the notify date already
                if (DateTime.Now > DateTime.Parse(reminder.NotifyAtDate).AddDays(7))
                {
                    await DeleteReminderAsync(reminder);
                    continue;
                }

                // update message
                var delivery = await inventoryService.GetDeliveryAsync(reminder.RelatedEntityId);
                var inv = await inventoryService.GetInventoryFromDeliveryAsync(delivery.Id);
                var item = await inventoryService.GetItemAsync(inv.ItemBarcode);

                var message = $"""
                The item {item.Name} delivered on {DateTime.Parse(delivery.DateDelivered):MM/dd/yyyy} is to be returned on {DateTime.Parse(delivery.ReturnByDate):MM/dd/yyyy}.
                Items Sold: {delivery.DeliveryAmount - inv.TotalAmount}
                Stock Remaining: {inv.TotalAmount}
                Amount to Pay: {(delivery.DeliveryAmount - inv.TotalAmount) * (delivery.BatchWorth / delivery.DeliveryAmount):0.00}
                """;

                reminder.Message = message;
            }
            else if (reminder.Type is ReminderType.SupplyRunout)
            {
                // check if restocked or not already
                var inv = await inventoryService.GetInventoryAsync(reminder.RelatedEntityId);
                if (inv is null) // inv record does not exist anymore (meaning stock has been depleted)
                {
                    await DeleteReminderAsync(reminder);
                    continue;
                }
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
                // resched reminder and local notification
                var item = await inventoryService.GetItemAsync(inv.ItemBarcode);
                var message = $"Stock for item {item.Name} is running low\nRemaining Stock: {totalInv}";
                reminder.Message = message;
                var notifyDate = DateTime.Parse(reminder.NotifyAtDate);
                if (notifyDate.Date < DateTime.Now.Date) // only when today's date (by day) is > previous notifydate sched
                {
                    reminder.NotifyAtDate = DateTime.Now.AddMinutes(2).ToString();
                    await ScheduleLocalNotification(reminder.Id, reminder.Title, reminder.Message, DateTime.Parse(reminder.NotifyAtDate), reminder.Id.ToString());
                }
            }
            else if (reminder.Type is ReminderType.DeliveryReminder)
            {
                if (DateTime.Parse(reminder.NotifyAtDate) < DateTime.Now)
                {
                    await DeleteReminderAsync(reminder);
                }
            }
        }
    }
}
