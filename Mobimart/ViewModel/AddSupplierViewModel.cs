using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobiMart.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MobiMart.ViewModel;

public partial class AddSupplierViewModel : ObservableObject
{
    [ObservableProperty]
    string name = "";

    [ObservableProperty]
    string type = "";

    [ObservableProperty]
    string email = "";

    [ObservableProperty]
    string socials = "";

    [ObservableProperty]
    string number = "";

    [RelayCommand]
    async Task saveChanges()
    {
        await Shell.Current.DisplayAlert(
            "Saved",
            $"Name: {name}\nType: {type}\nNumber: {number}\nEmail: {email}\nSocials: {socials}",
            "OK"
        );
    }
}