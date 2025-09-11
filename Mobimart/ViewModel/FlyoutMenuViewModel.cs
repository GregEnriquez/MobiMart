using System;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobiMart.Service;

namespace MobiMart.ViewModel;

public partial class FlyoutMenuViewModel : BaseViewModel
{
    UserService userService;
    BusinessService businessService;

    [ObservableProperty]
    string businessName = "Business Name";
    [ObservableProperty]
    string username = "username";

    public FlyoutMenuViewModel(UserService userService, BusinessService businessService)
    {
        this.userService = userService;
        this.businessService = businessService;

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
        await Shell.Current.GoToAsync("//MainPage", true);
        IsBusy = false;
    }


    public async Task UpdateInfo()
    {
        var userInstance = await userService.GetUserInstanceAsync();
        var user = await userService.GetUserAsync(userInstance.UserId);
        var business = await businessService.GetBusinessAsync(user.BusinessRefId);

        Username = "";
        BusinessName = "";

        Username = user.Email;
        BusinessName = business.Name;

        if (business is not null && user.EmployeeType is not null)
        {
            Username += $" |{user.EmployeeType}";
        }
    }
}
