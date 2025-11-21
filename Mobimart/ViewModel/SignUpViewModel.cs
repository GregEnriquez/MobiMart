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
        Title = "Sign Up";
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
        
        var navPage = Application.Current!.Windows[0].Page as NavigationPage;
        try
        {
            if (!await userService.RegisterUserAsync(Email, Password))
            {
                EmailHasError = true;
                IsBusy = false;
                return;
            }
        }
        catch (HttpRequestException e)
        {
            IsBusy = false;
            await navPage!.DisplayAlert("Connection Error", e.Message, "OK");
            return;
        }
        catch (TaskCanceledException e)
        {
            IsBusy = false;
            await navPage!.DisplayAlert("Timeout Error", e.Message, "OK");
            return;
        }
        catch (Exception e)
        {
            IsBusy = false;
            await navPage!.DisplayAlert("Error", e.Message, "OK");
            return;
        }

        bool answer = await navPage!.DisplayAlert("Success", "Account has been registered", "OK", "GO BACK");
        if (answer) await navPage!.PopAsync();

        IsBusy = false;
        Email = "";
        Password = "";
        ConfirmPassword = "";
        EmailHasError = false;
        PasswordHasError = false;
        ConfirmPasswordHasError = false;
    }
}
