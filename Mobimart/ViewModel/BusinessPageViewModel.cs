using System;
using System.Diagnostics;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobiMart.Service;

namespace MobiMart.ViewModel;

public partial class BusinessPageViewModel : BaseViewModel
{
    [ObservableProperty]
    string businessName = "";
    [ObservableProperty]
    string businessAddress = "";
    [ObservableProperty]
    string businessCode = "";

    [ObservableProperty]
    bool canViewEmployees = false;
    [ObservableProperty]
    bool joinedBusiness = false;
    [ObservableProperty]
    bool isOwner = true;

    UserService userService;
    BusinessService businessService;

    public BusinessPageViewModel(UserService userService, BusinessService businessService)
    {
        Title = "Business Page";
        this.userService = userService;
        this.businessService = businessService;

        // var userInstance = await userService.GetUserInstanceAsync();
        // var user = await userService.GetUserAsync(userInstance.UserId);
        // var business = await businessService.GetBusinessAsync(user.BusinessRefId);
        // var userInstance = userService.GetUserInstanceAsync().GetAwaiter().GetResult();
        // var user = userService.GetUserAsync(userInstance.UserId).GetAwaiter().GetResult();
        // var business = businessService.GetBusinessAsync(user.BusinessRefId).GetAwaiter().GetResult();

        // BusinessName = business.Name;
        // BusinessAddress = business.Address;
        // BusinessCode = business.Code;

        // if (user.EmployeeType == "owner")
        // {
        //     CanViewEmployees = true;
        // }
    }

    [RelayCommand]
    public async Task SaveChanges()
    {
        var userInstance = await userService.GetUserInstanceAsync();
        var user = await userService.GetUserAsync(userInstance.UserId);
        var business = await businessService.GetBusinessAsync(user.BusinessRefId);

        if (business is not null)
        {
            business.Name = BusinessName;
            business.Address = BusinessAddress;
            await businessService.UpdateBusinessAsync(business);
            return;
        }

        var random = new Random();
        string code;
        do
        {
            code = random.Next(10000, 99999).ToString();
        }
        while (await businessService.BusinessExistsAsync(code));
        BusinessCode = code;

        business = new()
        {
            Name = BusinessName,
            Address = BusinessAddress,
            Code = code,
            LastModified = DateTime.Today.ToString()
        };
        await businessService.AddBusinessAsync(business);
        user.BusinessRefId = business.Id;
        user.EmployeeType = "owner";
        await userService.UpdateUserAsync(user);

        await UpdateInfo();
    }


    public async Task UpdateInfo()
    {
        var userInstance = await userService.GetUserInstanceAsync();
        var user = await userService.GetUserAsync(userInstance.UserId);
        var business = await businessService.GetBusinessAsync(user.BusinessRefId);

        IsOwner = true;
        BusinessName = "";
        BusinessAddress = "";
        BusinessCode = "";
        JoinedBusiness = false;
        if (business is null) return;

        BusinessName = business.Name;
        BusinessAddress = business.Address;
        BusinessCode = business.Code;
        JoinedBusiness = true;

        CanViewEmployees = user.EmployeeType == "owner";
        IsOwner = user.EmployeeType == "owner";
    }


    [RelayCommand]
    public async Task CopyCode()
    {
        await Clipboard.Default.SetTextAsync(BusinessCode);
        await Toast.Make("Copied to clipboard", ToastDuration.Short, 14).Show();
    }
}
