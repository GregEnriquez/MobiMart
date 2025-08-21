using System;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobiMart.Service;

namespace MobiMart.ViewModel;

public partial class SignUpViewModel : BaseViewModel
{
    readonly UserService userService;

    [ObservableProperty]
    string email = "";
    [ObservableProperty]
    bool emailHasError = false;
    [ObservableProperty]
    string password = "";
    [ObservableProperty]
    bool passwordHasError = false;
    [ObservableProperty]
    string confirmPassword = "";
    [ObservableProperty]
    bool confirmPasswordHasError = false;


    public SignUpViewModel(UserService userService)
    {
        Title = "SAMPLE LANG HEHE";
        this.userService = userService;
    }


    [RelayCommand]
    async Task RegisterUser()
    {
        IsBusy = true;
        PasswordHasError = Password.Length < 8;
        ConfirmPasswordHasError = Password != ConfirmPassword;

        if (Password.Length < 8 || Password != ConfirmPassword)
        {
            IsBusy = false;
            return;
        }

        if (!await userService.RegisterUserAsync(Email, Password))
        {
            EmailHasError = true;
            IsBusy = false;
            return;
        }

        bool answer = await Shell.Current.DisplayAlert("Success", "Account has been registered", "OK", "GO BACK");
        if (answer) await Shell.Current.GoToAsync("..");

        IsBusy = false;
        Email = "";
        Password = "";
        ConfirmPassword = "";
        EmailHasError = false;
        PasswordHasError = false;
        ConfirmPasswordHasError = false;
    }
}
