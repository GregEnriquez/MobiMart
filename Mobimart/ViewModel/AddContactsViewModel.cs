using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MobiMart.ViewModel
{
    public partial class AddContactsViewModel : ObservableObject
    {
        [ObservableProperty]
        string cName = "";

        [ObservableProperty]
        string cEmail = "";

        [ObservableProperty]
        string cNumber = "";

        [ObservableProperty]
        string cSocials = "";

        [RelayCommand]
        async Task saveChanges()
        {
            await Shell.Current.DisplayAlert(
                "Saved",
                $"Name: {cName}\nEmail: {cEmail}\nNumber: {cNumber}\nSocials: {cSocials}",
                "OK"
            );
        }

    }
}
