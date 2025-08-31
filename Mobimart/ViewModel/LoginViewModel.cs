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

        Debug.WriteLine("i was initiated");
        Task.Run(async () =>
        {
            IsBusy = true;
            if (await userService.ResumeUserInstanceAsync())
            {
                await Shell.Current.GoToAsync("//UserPage", true);
                IsBusy = false;
            }
            IsBusy = false;
        });
    }


    [RelayCommand]
    async Task LoginUser()
    {
        try
        {
            IsBusy = true;
            await userService.LoginUserAsync(Email, Password);
        }
        catch (HttpRequestException e)
        {
            IsBusy = false;
            await Shell.Current.DisplayAlert("Connection Error", e.Message, "OK");
            return;
        }
        catch (TaskCanceledException e)
        {
            IsBusy = false;
            await Shell.Current.DisplayAlert("Timeout Error", e.Message, "OK");
            return;
        }
        catch (Exception e)
        {
            IsBusy = false;
            await Shell.Current.DisplayAlert("Error", e.Message, "OK");
            return;
        }

        // login is succesful (do something)
        await Shell.Current.GoToAsync("//UserPage", true);
        IsBusy = false;
    }


    [RelayCommand]
    async Task GoToSignup()
    {
        // await Navigation.PushAsync(new SignUpPage());
        Debug.WriteLine("I am here");
        await Shell.Current.GoToAsync($"{nameof(SignUpPage)}", true);
    }
}
