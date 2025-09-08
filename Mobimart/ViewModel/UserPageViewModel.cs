using System;
using System.Diagnostics;
using System.Globalization;
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

    UserService userService;

    public UserPageViewModel(UserService userService)
    {
        Title = "User Page";
        this.userService = userService;

        Task.Run(async () =>
        {
            IsBusy = true;
            
            var userInstance = await userService.GetUserInstanceAsync();
            var user = await userService.GetUserAsync(userInstance.UserId);

            FullName = user.FirstName + " " + user.LastName;
            Email = user.Email;
            Password = "********";
            Age = user.Age;
            // Birthday = DateOnly.ParseExact(user.BirthDate, "yyyy-MM-dd");
            Birthday = DateOnly.ParseExact(user.BirthDate, "yyyy-MM-dd").ToDateTime(TimeOnly.MinValue);
            PhoneNumber = user.PhoneNumber;
        
            IsBusy = false;
        });

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
        var birthday = DateOnly.FromDateTime(Birthday).ToString("yyyy-MM-dd");
        if (user.BirthDate != birthday)
            user.BirthDate = birthday;

        if (user.PhoneNumber != PhoneNumber)
                user.PhoneNumber = PhoneNumber;

        // business code

        // last modified


        await userService.UpdateUserAsync(user);
        IsBusy = false;
    }
}
