using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls.Handlers.Compatibility;
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
    SupplierService supplierService;

    public ViewTransactionViewModel(InventoryService inventoryService, SalesService salesService, SupplierService supplierService)
    {
        this.inventoryService = inventoryService;
        this.salesService = salesService;
        this.supplierService = supplierService;
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

        var businessId = Guid.Empty;
        if (Shell.Current.BindingContext is FlyoutMenuViewModel vm)
        {
            businessId = vm.BusinessId;
        }

        // return each item to inventory (put it on the inventory with the oldest delivery record of that item)
        for (int i = 0; i < Record.Items.Count; i++)
        {
            if (!selectedItems[i]) continue;
            var salesItem = Record.Items[i];

            // get all the delivery records for this item
            var itemsInDeliveries = (await inventoryService.GetDeliveriesAsync(salesItem.Barcode)).OrderByDescending(x => x.DateDelivered).ToList(); // latest to oldest

            // filter out consigned delivery items that have been returned already
            for (var j = 0; j < itemsInDeliveries.Count; j++)
            {
                var del = itemsInDeliveries[j];
                var sup = await supplierService.GetSupplierAsync(del.SupplierId);
                if (sup.Type.ToLower().Equals("consignment") && del.ReturnByDate == null) // if a consignment delivery's ReturnByDate is null, it has been returned already
                    itemsInDeliveries.Remove(del);
            }

            // find which delivery records will receive back the item
            var deliveriesToReturnTo = new List<Delivery>();
            // get the delivery records that has inventory space to spare
            int allocatedAmount = 0;
            foreach (var del in itemsInDeliveries)
            {
                if (allocatedAmount >= salesItem.Quantity) break; // we found enough inventory space to return the item

                var inv = await inventoryService.GetInventoryFromDeliveryAsync(del.Id);
                int allocatableInvAmount = inv is null ? del.DeliveryAmount : del.DeliveryAmount - inv.TotalAmount;
                
                if (allocatableInvAmount > 0) {
                    deliveriesToReturnTo.Add(del);
                    allocatedAmount += allocatableInvAmount;
                }
            }

            // update the invetory record of the allocatable inventory related to the deliveries
            var allInventory = await inventoryService.GetInventoryTable(); // to also look for soft-deleted inventory record
            int amountToReturn = salesItem.Quantity;
            foreach(var del in deliveriesToReturnTo) 
            {
                if (amountToReturn == 0) break;

                var inv = allInventory.Find(x => x.DeliveryId == del.Id);
                if (inv is null) // inventory is hard-deleted
                {
                    inv = new Inventory()
                    {
                        BusinessId = businessId,
                        DeliveryId = del.Id,
                        ItemBarcode = salesItem.Barcode,
                        TotalAmount = 0
                    };
                    await inventoryService.AddInventoryAsync(inv);
                    allInventory.Add(inv);
                }

                int allocatableInvAmount = 0;
                allocatableInvAmount = del.DeliveryAmount - inv.TotalAmount;
                if (amountToReturn - allocatableInvAmount > 0)
                {
                    inv.TotalAmount += allocatableInvAmount;
                    amountToReturn -= allocatableInvAmount;
                }
                else
                {
                    inv.TotalAmount += amountToReturn;
                    amountToReturn = 0;
                }

                if (inv.IsDeleted) inv.IsDeleted = false;
                await inventoryService.UpdateInventoryAsync(inv);
            }
 
            // update transaction total
            var transaction = await salesService.GetSalesTransactionAsync(Record.TransactionId);
            transaction.TotalPrice -= salesItem.Price;
            transaction.Change = transaction.Payment - transaction.TotalPrice;
            await salesService.UpdateSalesTransactionAsync(transaction);

            // delete transaction item
            await salesService.DeleteSalesItemTransactionAsync(salesItem);
        }

        // delete transaction record if there are no sales item
        if ((await salesService.GetSalesItemsAsync(Record.TransactionId)).Count <= 0)
        {
            await salesService.DeleteSalesTransactionAsync(Record.TransactionId);
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

        var businessId = Guid.Empty;
        if (Shell.Current.BindingContext is FlyoutMenuViewModel vm)
        {
            businessId = vm.BusinessId;
        }

        // return each item to inventory (place it on the last delivery record of that item)
        /* 'oldest delivery' because that the inventory record that gets remove when doing a transaction
        is the inventory with the oldest delivery record*/
        for (int i = 0; i < Record.Items.Count; i++)
        {
            var salesItem = Record.Items[i];

            // get all the delivery records for this item
            var itemsInDeliveries = (await inventoryService.GetDeliveriesAsync(salesItem.Barcode)).OrderByDescending(x => x.DateDelivered).ToList(); // latest to oldest

            // filter out consigned delivery items that have been returned already
            for (var j = 0; j < itemsInDeliveries.Count; j++)
            {
                var del = itemsInDeliveries[j];
                var sup = await supplierService.GetSupplierAsync(del.SupplierId);
                if (sup.Type.ToLower().Equals("consignment") && del.ReturnByDate == null) // if a consignment delivery's ReturnByDate is null, it has been returned already
                    itemsInDeliveries.Remove(del);
            }

            // find which delivery records will receive back the item
            var deliveriesToReturnTo = new List<Delivery>();
            // get the delivery records that has inventory space to spare
            int allocatedAmount = 0;
            foreach (var del in itemsInDeliveries)
            {
                if (allocatedAmount >= salesItem.Quantity) break; // we found enough inventory space to return the item

                var inv = await inventoryService.GetInventoryFromDeliveryAsync(del.Id);
                int allocatableInvAmount = inv is null ? del.DeliveryAmount : del.DeliveryAmount - inv.TotalAmount;
                
                if (allocatableInvAmount > 0) {
                    deliveriesToReturnTo.Add(del);
                    allocatedAmount += allocatableInvAmount;
                }
            }


            // update the invetory record of the allocatable inventory related to the deliveries
            var allInventory = await inventoryService.GetInventoryTable(); // to also look for soft-deleted inventory record
            int amountToReturn = salesItem.Quantity;
            foreach(var del in deliveriesToReturnTo) 
            {
                if (amountToReturn == 0) break;

                var inv = allInventory.Find(x => x.DeliveryId == del.Id);
                if (inv is null) // inventory is hard-deleted
                {
                    inv = new Inventory()
                    {
                        BusinessId = businessId,
                        DeliveryId = del.Id,
                        ItemBarcode = salesItem.Barcode,
                        TotalAmount = 0
                    };
                    allInventory.Add(inv);
                    await inventoryService.AddInventoryAsync(inv);
                }

                int allocatableInvAmount = 0;
                allocatableInvAmount = del.DeliveryAmount - inv.TotalAmount;
                if (amountToReturn - allocatableInvAmount > 0)
                {
                    inv.TotalAmount += allocatableInvAmount;
                    amountToReturn -= allocatableInvAmount;
                }
                else
                {
                    inv.TotalAmount += amountToReturn;
                    amountToReturn = 0;
                }

                if (inv.IsDeleted) inv.IsDeleted = false;
                await inventoryService.UpdateInventoryAsync(inv);
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
            if (i.IsDeleted) continue;
            selectedItems.Add(false);
        }
    }
}
