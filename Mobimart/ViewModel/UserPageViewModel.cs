using System;
using System.Diagnostics;
using System.Globalization;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobiMart.Service;

namespace MobiMart.ViewModel;

public partial class UserPageViewModel : BaseViewModel
{

    [ObservableProperty]
    string fullName = "";
    [ObservableProperty]
    string email = "";
    [ObservableProperty]
    string password = "";
    [ObservableProperty]
    int age;
    [ObservableProperty]
    DateTime birthday;
    [ObservableProperty]
    string phoneNumber = "";
    [ObservableProperty]
    string businessCode = "";
    [ObservableProperty]
    bool notJoinedBusiness = true;
    [ObservableProperty]
    bool isPending = false;

    UserService userService;
    BusinessService businessService;

    public UserPageViewModel(UserService userService, BusinessService businessService)
    {
        Title = "User Page";
        this.userService = userService;
        this.businessService = businessService;

        // Task.Run(async () =>
        // {
        //     IsBusy = true;

        //     var userInstance = await userService.GetUserInstanceAsync();
        //     var user = await userService.GetUserAsync(userInstance.UserId);

        //     FullName = user.FirstName + " " + user.LastName;
        //     Email = user.Email;
        //     Password = "********";
        //     Age = user.Age;
        //     // Birthday = DateOnly.ParseExact(user.BirthDate, "yyyy-MM-dd");
        //     Birthday = DateOnly.ParseExact(user.BirthDate, "yyyy-MM-dd").ToDateTime(TimeOnly.MinValue);
        //     PhoneNumber = user.PhoneNumber;

        //     IsBusy = false;
        // });

    }

    [RelayCommand]
    async Task SaveChanges()
    {
        IsBusy = true;
        var userInstance = await userService.GetUserInstanceAsync();
        var user = await userService.GetUserAsync(userInstance.UserId);


        var firstName = String.Join(" ", FullName.Split(" ").Take(FullName.Split(" ").Length - 1));
        if (user.FirstName != firstName)
            user.FirstName = firstName;

        var lastName = FullName.Split(" ").Last();
        if (user.LastName != lastName)
            user.LastName = lastName;

        if (user.Email != Email)
            user.Email = Email;

        // password

        if (user.Age != Age)
            user.Age = Age;

        // birthday
        if (user.BirthDate is not null && user.BirthDate.Value.Date != Birthday.Date)
            user.BirthDate = new DateTime(DateOnly.FromDateTime(Birthday), TimeOnly.FromDateTime(Birthday));

        if (user.PhoneNumber != PhoneNumber)
            user.PhoneNumber = PhoneNumber;

        // business code

        // last modified


        await userService.UpdateUserAsync(user);
        await Toast.Make("Profile Successfully Updated", ToastDuration.Short, 14).Show();
        IsBusy = false;
    }


    public async Task UpdateInfo()
    {
        var userInstance = await userService.GetUserInstanceAsync();
        if (userInstance is null) return; // no account is logged in or in the middle of logging out

        var user = await userService.GetUserAsync(userInstance.UserId);

        IsBusy = true;
        FullName = "";
        Email = "";
        Password = "";
        Age = 0;
        Birthday = DateOnly.MinValue.ToDateTime(TimeOnly.MinValue);
        PhoneNumber = "";
        BusinessCode = "";
        NotJoinedBusiness = true;
        IsPending = false;

        FullName = user.FirstName + " " + user.LastName;
        Email = user.Email;
        Password = "********";
        Age = user.Age;
        // Birthday = DateOnly.ParseExact(user.BirthDate, "yyyy-MM-dd").ToDateTime(TimeOnly.MinValue);
        Birthday = user.BirthDate is null ? DateTime.MinValue : user.BirthDate.Value;
        PhoneNumber = user.PhoneNumber;

        var business = await businessService.GetBusinessAsync(user.BusinessRefId);
        if (business is not null)
        {
            BusinessCode = business.Code;
            NotJoinedBusiness = false;
        }
        if (Shell.Current.BindingContext is FlyoutMenuViewModel vm)
        {
            NotJoinedBusiness = !vm.IsUserInBusiness;
        }
        
        IsPending = !NotJoinedBusiness && user.EmployeeType.Equals("pending");

        IsBusy = false;
    }


    [RelayCommand]
    public async Task JoinBusiness()
    {
        if (IsBusy) return;
        IsBusy = true;

        if (BusinessCode is null || !NotJoinedBusiness)
        {
            IsBusy = false;
            return;
        }

        var userInstance = await userService.GetUserInstanceAsync();
        var user = await userService.GetUserAsync(userInstance.UserId);
        var business = await businessService.GetBusinessAsync(BusinessCode);

        if (business is null)
        {
            await Shell.Current.DisplayAlert("Error", "Business Code does not exist.", "OK");
            IsBusy = false;
            return;
        }

        bool confirm = await Shell.Current.DisplayAlert(
            "Confirm Join",
            $"Are you sure you want to join the the business named {business.Name}?",
            "Yes", "No"
        );

        if (!confirm)
        {
            IsBusy = false;
            return;
        }

        user.BusinessRefId = business.Id;
        user.EmployeeType = "pending";
        await userService.UpdateUserAsync(user);
        await UpdateInfo();
        NotJoinedBusiness = false;
        IsPending = true;


        IsBusy = false;
    }


    [RelayCommand]
    public async Task CancelRequest()
    {
        if (IsBusy) return;
        IsBusy = true;

        var userInstance = await userService.GetUserInstanceAsync();
        var user = await userService.GetUserAsync(userInstance.UserId);
        var business = await businessService.GetBusinessAsync(BusinessCode);

        bool confirm = await Shell.Current.DisplayAlert(
            "Confirm Join",
            $"Are you sure you want to cancel your request to join {business.Name}?",
            "Yes", "No"
        );

        if (confirm)
        {
            user.EmployeeType = "";
            user.BusinessRefId = Guid.Empty;
            await userService.UpdateUserAsync(user);
            await UpdateInfo();
            NotJoinedBusiness = true;
            IsPending = false;
        }

        IsBusy = false;        
    }
}
