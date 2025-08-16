using System;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using MobiMart.View;

namespace MobiMart.ViewModel;

public partial class LoginViewModel : BaseViewModel
{
    public LoginViewModel()
    {

    }

    [RelayCommand]
    async Task GoToSignup()
    {
        // await Navigation.PushAsync(new SignUpPage());
        Debug.WriteLine("I am here");
        await Shell.Current.GoToAsync($"{nameof(SignUpPage)}", true);
    }
}
