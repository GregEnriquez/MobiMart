using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobiMart.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobiMart.ViewModel
{
    public partial class EditSuppInventoryViewModel : ObservableObject
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

        public void loadItems(WholesaleInventory inv)
        {
            DeliveryId = inv.deliveryId;
            WItemName = inv.wItemName;
            WDelivQuantity = inv.wDelivQuantity;
            WDateDelivered = inv.wDateDelivered;
            WDateExpire = inv.wDateExpire;
            WBatchWorth = inv.wBatchWorth;
            WItemType = inv.wItemType;
            WItemDesc = inv.wItemDesc;
        }

        public void saveItems(WholesaleInventory inv)
        {
            inv.deliveryId = DeliveryId;
            inv.wItemName = WItemName;
            inv.wDelivQuantity = WDelivQuantity;
            inv.wDateDelivered = WDateDelivered;
            inv.wDateExpire = WDateExpire;
            inv.wBatchWorth = WBatchWorth;
            inv.wItemType = WItemType;
            inv.wItemDesc = WItemDesc;
        }

        [RelayCommand]
        async Task SaveChanges()
        {
            await Shell.Current.DisplayAlert(
               "Saved",
               $"Item: {WItemName} \nQuantity: {WDelivQuantity} \nDelivered: {WDateDelivered:d} \nExpires: {WDateExpire:d} \nWorth: {WBatchWorth} \nType: {WItemType} \nDescription: {WItemDesc}",
               "OK"
           );
        }
    }
}
