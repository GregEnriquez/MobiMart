using System;
using System.Diagnostics;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
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

        IsBusy = true;
    }


    [RelayCommand]
    async Task LoginUser()
    {
        var navPage = Application.Current!.Windows[0].Page as NavigationPage;
        try
        {
            IsBusy = true;
            await userService.LoginUserAsync(Email, Password);
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

        // login is succesful (do something)
        Email = "";
        Password = "";
        await Toast.Make("Login Succesful", ToastDuration.Short, 14).Show();

        // await Shell.Current.GoToAsync("//UserPage", true);
        await Task.Delay(500); //fake loading
        App.SwitchToShell();
        
        IsBusy = false;
    }


    [RelayCommand]
    async Task GoToSignup()
    {
        // Debug.WriteLine("I am here");
        // await Shell.Current.GoToAsync($"{nameof(SignUpPage)}", true);
        // await Task.Delay(500); //fake loading
        // App.SwitchToRegister();

        var signUpPage = IPlatformApplication.Current!.Services.GetRequiredService<SignUpPage>();

        if (Application.Current!.Windows[0].Page is NavigationPage navPage)
        {
            await navPage.PushAsync(signUpPage);
        }
    }


    public async Task OnAppearing()
    {
        IsBusy = true;
        // await userService.LogoutUserAsync();
        if (await userService.ResumeUserInstanceAsync())
        {
            await Toast.Make("Login Session Resumed", ToastDuration.Short, 14).Show();

            // await Shell.Current.GoToAsync("//UserPage", false);
            await Task.Delay(500); //fake loading
            App.SwitchToShell();

            IsBusy = false;
            return;
        }
        IsBusy = false;
    }
}
