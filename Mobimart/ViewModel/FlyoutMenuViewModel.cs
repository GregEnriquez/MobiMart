using System;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobiMart.Model;
using MobiMart.Service;

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

    User? user;
    Business? business;

    bool isRemindersUpdated = false;

    public FlyoutMenuViewModel(UserService userService, BusinessService businessService, NotificationService notificationService, InventoryService inventoryService)
    {
        this.userService = userService;
        this.businessService = businessService;
        this.notificationService = notificationService;
        this.inventoryService = inventoryService;

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

        await Shell.Current.GoToAsync("//MainPage", true);
        IsBusy = false;
    }


    public async Task UpdateInfo()
    {
        if (user is null)
        {
            var userInstance = await userService.GetUserInstanceAsync();
            if (userInstance is null) return;
            user = await userService.GetUserAsync(userInstance.UserId);
            business = await businessService.GetBusinessAsync(user.BusinessRefId);
        }

        Username = "";
        BusinessName = "";
        BusinessId = -1;

        Username = user.Email;
        BusinessName = business!.Name;
        BusinessId = business.Id;

        if (business is not null && user.EmployeeType is not null)
        {
            Username += $" |{user.EmployeeType}";
            IsUserInBusiness = true;
            IsUserOwner = user.EmployeeType.ToLower().Equals("owner");
        }
        else
        {
            IsUserInBusiness = false;
            IsUserOwner = false;
        }

        // update reminders
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
