using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobiMart.Model;
using MobiMart.Service;

namespace MobiMart.ViewModel;

public partial class ViewTransactionViewModel : BaseViewModel
{

    [ObservableProperty]
    SalesRecord record;
    [ObservableProperty]
    bool isReturning = false;
    List<bool> selectedItems = [];

    InventoryService inventoryService;
    SalesService salesService;

    public ViewTransactionViewModel(InventoryService inventoryService, SalesService salesService)
    {
        this.inventoryService = inventoryService;
        this.salesService = salesService;
    }


    [RelayCommand]
    public async Task Button1Clicked()
    {
        if (!IsReturning)
        {
            IsReturning = true;
            return;
        }
        
        // input validation
        bool confirm = await Shell.Current.DisplayAlert(
            "Return Selected Items?",
            "Are you sure you want to completely remove these items from this transaction record? This will remove them from the database and return the items back to the inventory. You only want to do this when the user returned these items from this transaction record for a refund.",
            "Yes", "No"
        );

        if (!confirm) return;
        if (IsBusy) return;
        IsBusy = true;

        int businessId = -1;
        if (Shell.Current.BindingContext is FlyoutMenuViewModel vm)
        {
            businessId = vm.BusinessId;
        }

        // return each item to inventory (place it on the last delivery record of that item)
        for (int i = 0; i < Record.Items.Count; i++)
        {
            if (!selectedItems[i]) continue;
            var salesItem = Record.Items[i];
            var itemsInDeliveries = await inventoryService.GetDeliveriesAsync(salesItem.Barcode);
            var latestDelivery = itemsInDeliveries.OrderBy(x => DateTime.Parse(x.DateDelivered)).ToList()[0];

            // update the inventory record of the latest delivery
            var invItem = await inventoryService.GetInventoryFromDeliveryAsync(latestDelivery.Id);
            if (invItem is not null)
            {
                invItem.TotalAmount += salesItem.Quantity;
                await inventoryService.UpdateInventoryAsync(invItem);
            }
            // or add a new one if the inventory for that delivery record is not present already
            else
            {
                invItem = new Inventory()
                {
                    BusinessId = businessId,
                    DeliveryId = latestDelivery.Id,
                    ItemBarcode = salesItem.Barcode,
                    TotalAmount = salesItem.Quantity
                };
                await inventoryService.AddInventoryAsync(invItem);
            }

            // update transaction total
            var transaction = await salesService.GetSalesTransactionAsync(Record.TransactionId);
            transaction.TotalPrice -= salesItem.Price;
            transaction.Change = transaction.Payment - transaction.TotalPrice;
            await salesService.UpdateSalesTransactionAsync(transaction);

            // delete transaction item
            await salesService.DeleteSalesItemTransactionAsync(salesItem);
        }

        await Shell.Current.GoToAsync("..");
        IsReturning = false;
        IsBusy = false;
    }


    [RelayCommand]
    public async Task Button2Clicked()
    {
        if (IsReturning)
        {
            IsReturning = false;
            // uncheck checkboxes
            return;
        }

        // void validation
        bool confirm = await Shell.Current.DisplayAlert(
            "Void Whole Transaction?",
            "Are you sure you want to completely remove this transaction record? This will remove the transaction from the database and return the items back to the inventory. You only want to do this when the user returns all the items from a transaction record for a full refund.",
            "Yes", "No"
        );

        if (!confirm) return;
        if (IsBusy) return;
        IsBusy = true;

        int businessId = -1;
        if (Shell.Current.BindingContext is FlyoutMenuViewModel vm)
        {
            businessId = vm.BusinessId;
        }

        // return each item to inventory (place it on the last delivery record of that item)
        for (int i = 0; i < Record.Items.Count; i++)
        {
            var salesItem = Record.Items[i];
            var itemsInDeliveries = await inventoryService.GetDeliveriesAsync(salesItem.Barcode);
            var latestDelivery = itemsInDeliveries.OrderBy(x => DateTime.Parse(x.DateDelivered)).ToList()[0];

            // update the inventory record of the latest delivery
            var invItem = await inventoryService.GetInventoryFromDeliveryAsync(latestDelivery.Id);
            if (invItem is not null)
            {
                invItem.TotalAmount += salesItem.Quantity;
                await inventoryService.UpdateInventoryAsync(invItem);
            }
            else
            {
                var inv = new Inventory()
                {
                    BusinessId = businessId,
                    DeliveryId = latestDelivery.Id,
                    ItemBarcode = salesItem.Barcode,
                    TotalAmount = salesItem.Quantity
                };
                await inventoryService.AddInventoryAsync(inv);
            }

            // delete transaction item
            await salesService.DeleteSalesItemTransactionAsync(salesItem);
        }

        // delete transaction
        await salesService.DeleteSalesTransactionAsync(Record.TransactionId);

        await Shell.Current.GoToAsync("..");
        IsBusy = false;
    }


    public void OnCheckboxClicked(CheckBox cb, SalesItem item)
    {
        selectedItems[Record.Items.IndexOf(item)] = cb.IsChecked;
    }



    public void UpdateSelectedRecords()
    {
        selectedItems = [];
        foreach (SalesItem i in Record.Items)
        {
            selectedItems.Add(false);
        }
    }
}
