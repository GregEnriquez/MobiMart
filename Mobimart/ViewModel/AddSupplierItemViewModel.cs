using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobiMart.ViewModel
{
    public partial class AddSupplierItemViewModel : ObservableObject
    {
        [ObservableProperty]
        int deliveryId;

        [ObservableProperty]
        int wDelivQuantity;

        [ObservableProperty]
        string wItemName = "";

        [ObservableProperty]
        decimal wBatchWorth;

        [ObservableProperty]
        string wItemType = "";

        [ObservableProperty]
        string wItemDesc = "";

        [ObservableProperty]
        DateTime wDateDelivered;

        [ObservableProperty]
        DateTime wDateExpire;

        [RelayCommand]
        async Task SaveChanges()
        {
            await Shell.Current.DisplayAlert(
               "Saved",
               $"Item: {wItemName} \nQuantity: {wDelivQuantity} \nDelivered: {wDateDelivered:d} \nExpires: {wDateExpire:d} \nWorth: {wBatchWorth} \nType: {wItemType} \nDescription: {wItemDesc}",
               "OK"
           );
        }
    }
}
