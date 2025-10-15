using System;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobiMart.Model;
using MobiMart.Service;
using MobiMart.View;

namespace MobiMart.ViewModel;

// [QueryProperty(nameof(ItemBarcode), "ItemBarcode")]
public partial class EditInventoryPopupViewModel : BaseViewModel, IQueryAttributable
{

    [ObservableProperty]
    public List<DeliveryRecord> deliveries;
    [ObservableProperty]
    public Item item;

    public string ItemBarcode { get; set; }

    InventoryService inventoryService;

    public EditInventoryPopupViewModel(InventoryService inventoryService, string barcode)
    {
        this.inventoryService = inventoryService;
        ItemBarcode = barcode;
        RefreshRecords();
    }


    private async void RefreshRecords()
    {
        Item = await inventoryService.GetItemAsync(ItemBarcode);
        // var DeliveriesDummy = new List<Delivery>();
        // var inventoryList = await inventoryService.GetInventoriesAsync(ItemBarcode);
        // var deliveries = await inventoryService.GetDeliveriesAsync(ItemBarcode);

        // foreach (var inv in inventoryList)
        // {
        //     var deliveryRecord = deliveries.Find(x => x.Id == inv.DeliveryId);
        //     if (deliveryRecord is not null)
        //     {
        //         DeliveriesDummy.Add(deliveryRecord);
        //         continue;
        //     }
        // }

        // Deliveries = DeliveriesDummy;
        try
        {
            var deliveries = await inventoryService.GetDeliveryRecordsViaItem(ItemBarcode);
            var filteredDeliveries = new List<DeliveryRecord>();
            for (int i = 0; i < deliveries.Count; i++)
            {
                var d = deliveries[i];
                var inv = await inventoryService.GetInventoryFromDeliveryAsync(d.DeliveryId);
                if (inv is null) continue;
                filteredDeliveries.Add(d);
            }

            Deliveries = [.. filteredDeliveries.AsEnumerable()];
        }
        catch (Exception e)
        {
            
        }
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("ItemBarcode"))
        {
            ItemBarcode = query["ItemBarcode"].ToString()!;
        }
    }
}
