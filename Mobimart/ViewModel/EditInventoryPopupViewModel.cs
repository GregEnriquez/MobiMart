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
    [ObservableProperty]
    public string selectedSortOption;

    public List<string> SortOptions { get; } = [
        "Expiry Date (Soonest)",
        "Expiry Date (Latest)",
        "Date Delivered (Newest)",
        "Date Delivered (Oldest)",
        "Quantity (High to Low)",
        "Quantity (Low to High)"
    ];

    private List<DeliveryRecord> _allDeliveriesCache = [];

    public string ItemBarcode { get; set; }

    InventoryService inventoryService;

    public EditInventoryPopupViewModel(InventoryService inventoryService, string barcode)
    {
        this.inventoryService = inventoryService;
        ItemBarcode = barcode;
        RefreshRecords();
    }


    partial void OnSelectedSortOptionChanged(string value)
    {
        SortData(value);
    }


    private void SortData(string sortType)
    {
        if (_allDeliveriesCache is null || !_allDeliveriesCache.Any()) return;

        // Sort based on the selected string
        var sorted = sortType switch
        {
            "Date Delivered (Newest)" => _allDeliveriesCache.OrderByDescending(x => x.DateDelivered),
            "Date Delivered (Oldest)" => _allDeliveriesCache.OrderBy(x => x.DateDelivered),
            "Quantity (High to Low)" => _allDeliveriesCache.OrderByDescending(x => x.QuantityInStock), // Or DelivQuantity depending on preference
            "Quantity (Low to High)" => _allDeliveriesCache.OrderBy(x => x.QuantityInStock),
            "Expiry Date (Soonest)" => _allDeliveriesCache.OrderBy(x => x.DateExpire),
            "Expiry Date (Latest)" => _allDeliveriesCache.OrderByDescending(x => x.DateExpire),
            _ => _allDeliveriesCache.AsEnumerable()
        };

        // update observable property
        Deliveries = [.. sorted];
    }


    private async void RefreshRecords()
    {
        Item = await inventoryService.GetItemAsync(ItemBarcode);

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

            // save to cache
            _allDeliveriesCache = filteredDeliveries;

            // set default view
            SelectedSortOption = SortOptions[0];
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
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
