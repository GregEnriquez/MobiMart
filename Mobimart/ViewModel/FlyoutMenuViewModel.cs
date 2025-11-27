using System;
using System.Diagnostics;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobiMart.Model;
using MobiMart.Service;
using MobiMart.View;

namespace MobiMart.ViewModel;

public partial class FlyoutMenuViewModel : BaseViewModel
{
    UserService userService;
    BusinessService businessService;
    NotificationService notificationService;
    InventoryService inventoryService;
    SyncService syncService;

    [ObservableProperty]
    string businessName = "Business Name";
    [ObservableProperty]
    string username = "username";
    [ObservableProperty]
    Guid businessId = Guid.Empty;
    [ObservableProperty]
    bool isUserInBusiness = false;
    [ObservableProperty]
    bool isUserOwner = false;
    [ObservableProperty]
    bool isUserEmployee = false;

    [ObservableProperty]
    string lastSyncDisplay = "Sync Now";

    User? user;
    Business? business;

    bool isRemindersUpdated = false;

    public FlyoutMenuViewModel(UserService userService, BusinessService businessService, NotificationService notificationService, InventoryService inventoryService, SyncService syncService)
    {
        this.userService = userService;
        this.businessService = businessService;
        this.notificationService = notificationService;
        this.inventoryService = inventoryService;
        this.syncService = syncService;

        user = null;

        // Task.Run(async () =>
        // {
        //     var userInstance = await userService.GetUserInstanceAsync();
        //     var user = await userService.GetUserAsync(userInstance.UserId);
        //     var business = await businessService.GetBusinessAsync(user.BusinessRefId);

        //     username = user.Email;
        //     businessName = business.Name;
        // });
    }


    [RelayCommand]
    public async Task Logout()
    {
        if (IsBusy) return;
        IsBusy = true;

        try
        {
            await userService.LogoutUserAsync();
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.StackTrace);
        }

        user = null;
        business = null;
        IsUserInBusiness = false;
        IsUserOwner = false;
        BusinessId = Guid.Empty;

        // await Shell.Current.GoToAsync("///MainPage", true);
        await Toast.Make("Account Logged Out", ToastDuration.Short, 14).Show();
        Shell.Current.FlyoutIsPresented = false;
        await Task.Delay(500); //fake loading
        App.SwitchToLogin();

        IsBusy = false;
    }


    public async Task UpdateInfo()
    {
        var userInstance = await userService.GetUserInstanceAsync();
        if (userInstance is null) return;
        user = await userService.GetUserAsync(userInstance.UserId);
        business = await businessService.GetBusinessAsync(user.BusinessId);

        Username = "";
        BusinessName = "";
        BusinessId = Guid.Empty;

        Username = user.Email;

        if (business is not null)
        {
            if (!user.EmployeeType.Equals(""))
            {
                Username += $" |{user.EmployeeType}";
                IsUserOwner = user.EmployeeType.ToLower().Equals("owner");
                IsUserInBusiness = true;
                IsUserEmployee = IsUserOwner || user.EmployeeType.ToLower().Equals("employee");

                BusinessName = business!.Name;
                BusinessId = business.Id;
            }
            else if (user.EmployeeType.Equals(""))
            {
                IsUserInBusiness = false;
                IsUserOwner = false;
                IsUserEmployee = false;
            }
        }
        else
        {
            IsUserInBusiness = false;
            IsUserOwner = false;
            IsUserEmployee = false;
        }

        RefreshSyncTimeDisplay();

        // update reminders only once a day
        if (!isRemindersUpdated)
        {
            await notificationService.CheckAndScheduleNotificationsAsync(inventoryService);
            isRemindersUpdated = true;
        }
    }



    [RelayCommand]
    public async Task SyncNow()
    {
        if (IsBusy) return;
        IsBusy = true;

        // show loading user feedback
        var popup = new SyncBusyPopup();
        Shell.Current.ShowPopup(popup); 

        try 
        {
            // short delay to ensure popup renders smoothly
            await Task.Delay(100);
            
            bool success = await syncService.SyncDataAsync();

            // close popup
            await popup.CloseAsync();

            if (success)
            {
                await Toast.Make("Sync Complete!", ToastDuration.Short).Show();

                // refresh other pages data on screen (by going back to the user page)
                await Shell.Current.GoToAsync("//UserPage");

                // refresh data on screen
                await ForceUpdateInfo();
            }
            else
            {
                await Toast.Make("Sync Failed. Check internet.", ToastDuration.Long).Show();
            }
        }
        catch (Exception ex)
        {
            await popup.CloseAsync(); // Ensure popup closes even on crash
            Debug.WriteLine(ex);
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void RefreshSyncTimeDisplay()
    {
        var lastSyncStr = Preferences.Get("LastSyncTimestamp", "");
        
        if (string.IsNullOrEmpty(lastSyncStr) || lastSyncStr == DateTimeOffset.MinValue.ToString("o"))
        {
            LastSyncDisplay = "Sync Data Now\n(Never Synced)";
        }
        else
        {
            var date = DateTimeOffset.Parse(lastSyncStr).ToLocalTime();
            
            // Format: "Sync Now [Newline] Last: Nov 25, 10:30 PM"
            LastSyncDisplay = $"Sync Data Now\nLast: {date:MMM dd, h:mm tt}";
        }
    }


    public async Task ForceUpdateInfo()
    {
        user = null;
        await UpdateInfo();
    }
}
