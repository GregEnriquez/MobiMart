using System;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobiMart.Model;
using MobiMart.Service;

namespace MobiMart.ViewModel;

public partial class AddDeliveryReminderViewModel : BaseViewModel
{

    NotificationService notificationService;
    SupplierService supplierService;

    [ObservableProperty]
    List<Supplier> suppliers = [];
    [ObservableProperty]
    Supplier selectedSupplier;
    [ObservableProperty]
    DateTime notifyAtDate;
    [ObservableProperty]
    TimeSpan notifyAtTime = DateTime.Now.TimeOfDay;
    [ObservableProperty]    
    string reminderTitle;
    [ObservableProperty]
    string reminderMessage;

    int emptyCount = 0;

    public AddDeliveryReminderViewModel(NotificationService notificationService, SupplierService supplierService)
    {
        this.notificationService = notificationService;
        this.supplierService = supplierService;

        PropertyChanged += (s, e) =>
        {
            if (e.PropertyName is nameof(SelectedSupplier) or nameof(NotifyAtDate))
            {
                UpdateMessage();
            }
        };

        NotifyAtDate = DateTime.Now;
        ReminderTitle = "Delivery Reminder";
    }

    void UpdateMessage()
    {
        string? supplierName = SelectedSupplier is null ? null : SelectedSupplier.Name;

        ReminderMessage = $"Reminder that supplier {supplierName ?? "(select one)"} " +
                          $"will make a delivery on this day: {NotifyAtDate:MM/dd/yyyy}";
    }


    [RelayCommand]
    async Task SaveReminder()
    {
        // input validation
        emptyCount = 0;
        if (SelectedSupplier is null) emptyCount += 1;
        if (NotifyAtDate.Date < DateTime.Today.Date) emptyCount += 1;
        if (ReminderTitle.Equals("")) emptyCount += 1;
        if (ReminderMessage.Equals("")) emptyCount += 1;

        if (emptyCount > 0)
        {
            await Toast.Make("Make sure all fields are filled properly").Show();
            return;
        }


        int businessId = -1;
        if (Shell.Current.BindingContext is FlyoutMenuViewModel vm)
        {
            businessId = vm.BusinessId;
        }

        // save to database
        var r = new Reminder()
        {
            BusinessId = businessId,
            Type = ReminderType.DeliveryReminder,
            Title = ReminderTitle,
            Message = ReminderMessage,
            NotifyAtDate = (NotifyAtDate.Date + NotifyAtTime).ToString(),
            RepeatDaily = false,
            IsEnabled = true,
            Sent = false
        };
        await notificationService.AddReminderAsync(r);

        // schedule local notification
        await notificationService.ScheduleLocalNotification(
            r.Id, r.Title, r.Message, DateTime.Parse(r.NotifyAtDate).AddHours(-1), r.Id.ToString()
        );

        // Combine date and time
        var notifyAt = NotifyAtDate.Date + NotifyAtTime;
        await Toast.Make($"Reminder scheduled for {notifyAt} has been saved", ToastDuration.Short, 14).Show();

        // clear fields
        SelectedSupplier = null;
        NotifyAtDate = DateTime.Now;
        NotifyAtTime = DateTime.Now.TimeOfDay;

        // go back
        await Shell.Current.GoToAsync("..");
    }



    public async Task OnAppearing()
    {
        if (Suppliers.Count > 0) return;
        Suppliers = await supplierService.GetAllSuppliersAsync();
    }


}
