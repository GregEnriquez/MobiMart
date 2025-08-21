using System;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobiMart.Service;
using MobiMart.View;

namespace MobiMart.ViewModel;

public partial class LoginViewModel : BaseViewModel
{
    [ObservableProperty]
    string email = "";
    [ObservableProperty]
    string password = "";

    UserService userService;
    public LoginViewModel(UserService userService)
    {
        this.userService = userService;
    }


    [RelayCommand]
    async Task LoginUser()
    {
        await userService.LoginUserAsync(Email, Password);
    }


    [RelayCommand]
    async Task GoToSignup()
    {
        // await Navigation.PushAsync(new SignUpPage());
        Debug.WriteLine("I am here");
        await Shell.Current.GoToAsync($"{nameof(SignUpPage)}", true);
    }
}
