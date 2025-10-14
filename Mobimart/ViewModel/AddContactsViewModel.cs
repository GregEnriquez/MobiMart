using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MobiMart.ViewModel
{
    public partial class AddContactsViewModel : BaseViewModel
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
        async Task SaveChanges()
        {
            // input validation
            int emptyCount = 0;
            if (CName.Equals("")) emptyCount += 1;
            if (
                CEmail.Equals("") &&
                CNumber.Equals("") &&
                CSocials.Equals("")
            ) emptyCount += 1;

            if (emptyCount > 0)
            {
                await Toast.Make("Make sure to fill out the name and atleast one contact info", ToastDuration.Short, 14).Show();
                return;
            }

            // SAVE TO DATABASE


            await Shell.Current.GoToAsync("..");
        }

    }
}
