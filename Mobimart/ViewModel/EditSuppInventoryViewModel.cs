using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobiMart.Model;
using MobiMart.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace MobiMart.ViewModel
{
    public partial class EditSuppInventoryViewModel : BaseViewModel
    {
        [ObservableProperty]
        string barcodeId = "";
        [ObservableProperty]
        string wItemName = "";
        [ObservableProperty]
        string wItemDesc = "";
        [ObservableProperty]
        string wItemType = "";
        [ObservableProperty]
        float? wRetailPrice;
        [ObservableProperty]
        DateTime? wDateDelivered;
        [ObservableProperty]
        DateTime? wDateExpire;
        [ObservableProperty]
        int? wItemQuantity;
        [ObservableProperty]
        float? wBatchCost;
        [ObservableProperty]
        float? wUnitCost;
        [ObservableProperty]
        bool isBatchCost = true;

        [ObservableProperty]
        int deliveryId;

        Item item;
        Inventory inventory;
        Delivery delivery;
        Description desc;


        InventoryService? inventoryService;
        public EditSuppInventoryViewModel(InventoryService inventoryService)
        {
            this.inventoryService = inventoryService;
        }


        public async Task OnAppearing()
        {
            delivery = await inventoryService!.GetDeliveryAsync(DeliveryId);
            inventory = await inventoryService.GetInventoryFromDeliveryAsync(DeliveryId);
            item = await inventoryService.GetItemAsync(inventory.ItemBarcode);
            desc = await inventoryService.GetItemDescAsync(item.Barcode);


            BarcodeId = item.Barcode;
            WItemName = item.Name;
            WItemDesc = desc.Text;
            WItemType = item.Type;
            WRetailPrice = item.RetailPrice;
            WDateDelivered = DateTime.Parse(delivery.DateDelivered);
            WDateExpire = DateTime.Parse(delivery.ExpirationDate);
            WItemQuantity = inventory.TotalAmount;
        }


        [RelayCommand]
        async Task Delete()
        {
            bool confirm = await Shell.Current.DisplayAlert(
                "Confirm Delete",
                "Are you sure you want to delete this record from the inventory?",
                "Yes", "No"
            );

            if (!confirm) return;
            await inventoryService!.DeleteInventory(inventory);
            await Toast.Make("Record has been deleted", ToastDuration.Short, 14).Show();
            await Shell.Current.GoToAsync("..", true);
        }



        [RelayCommand]
        async Task SaveChanges()
        {
            var editedFields = new List<string>();

            if (!item.Name.Equals(WItemName)) editedFields.Add("Name");
            if (!desc.Text.Equals(WItemDesc)) editedFields.Add("Description");
            if (!item.Type.Equals(WItemType)) editedFields.Add("Type");
            if (item.RetailPrice != WRetailPrice) editedFields.Add("Retail Price");

            if (editedFields.Count > 0)
            {
                string m = "These item details have been modified:\n\n";
                editedFields.ForEach(warning => m += "    - " + warning + "\n");
                m += "\n\nAre you sure you want to save these changes?";
                bool confirm = await Shell.Current.DisplayAlert(
                    "Confirm Modify Item Details",
                    m,
                    "Yes", "No"
                );

                if (!confirm)
                {
                    WItemName = item.Name;
                    WItemType = item.Type;
                    WRetailPrice = item.RetailPrice;
                    WItemDesc = desc.Text;
                    return;
                }
                else
                {
                    if (editedFields.Contains("Description"))
                    {
                        desc.Text = WItemDesc;
                        await inventoryService!.UpdateDescAsync(desc);
                    }
                    item.Name = WItemName;
                    item.Type = WItemType;
                    item.RetailPrice = (float)WRetailPrice!;
                    await inventoryService!.UpdateItemAsync(item);
                }
            }


            delivery.DateDelivered = WDateDelivered.ToString()!;
            delivery.ExpirationDate = WDateExpire.ToString()!;
            inventory.TotalAmount = (int)WItemQuantity!;

            await inventoryService!.UpdateDescAsync(desc);
            await inventoryService!.UpdateDeliveryAsync(delivery);
            await inventoryService!.UpdateInventoryAsync(inventory);


            BarcodeId = "";
            WItemName = "";
            WItemDesc = "";
            WItemType = "";
            WRetailPrice = null;
            WDateDelivered = null;
            WDateExpire = null;
            WItemQuantity = null;

            await Toast.Make("Record has been updated", ToastDuration.Short, 14).Show();
            await Shell.Current.GoToAsync("..", true);
        }
    }
}
