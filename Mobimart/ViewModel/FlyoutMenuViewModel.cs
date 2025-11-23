using System;
using System.Diagnostics;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
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

    [ObservableProperty]
    string businessName = "Business Name";
    [ObservableProperty]
    string username = "username";
    [ObservableProperty]
    int businessId = -1;
    [ObservableProperty]
    bool isUserInBusiness = false;
    [ObservableProperty]
    bool isUserOwner = false;
    [ObservableProperty]
    bool isUserEmployee = false;

    User? user;
    Business? business;

    bool isRemindersUpdated = false;

    public FlyoutMenuViewModel(UserService userService, BusinessService businessService, NotificationService notificationService, InventoryService inventoryService)
    {
        this.userService = userService;
        this.businessService = businessService;
        this.notificationService = notificationService;
        this.inventoryService = inventoryService;

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
        business = await businessService.GetBusinessAsync(user.BusinessRefId);

        Username = "";
        BusinessName = "";
        BusinessId = -1;

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

        // update reminders only once a day
        if (!isRemindersUpdated)
        {
            await notificationService.CheckAndScheduleNotificationsAsync(inventoryService);
            isRemindersUpdated = true;
        }
    }


    public async Task ForceUpdateInfo()
    {
        user = null;
        await UpdateInfo();
    }
}
