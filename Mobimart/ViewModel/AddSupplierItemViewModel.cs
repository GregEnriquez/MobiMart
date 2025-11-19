using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobiMart.Model;
using MobiMart.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobiMart.ViewModel
{
    public partial class AddSupplierItemViewModel : BaseViewModel
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
        DateTime wDateDelivered;
        [ObservableProperty]
        DateTime? wDateExpire;
        [ObservableProperty]
        int? wDelivQuantity;
        [ObservableProperty]
        float? wBatchCost;
        [ObservableProperty]
        float? wUnitCost;
        [ObservableProperty]
        bool isBatchCost = true;
        [ObservableProperty]
        string wConsignmentSchedule = "";
        [ObservableProperty]
        DateTime? wReturnByDate;

        [ObservableProperty]
        bool isScannerVisible = false;
        [ObservableProperty]
        Supplier supplier;
        [ObservableProperty]
        bool isFromInventory = false;
        [ObservableProperty]
        bool isFromScanBarcode;

        InventoryService inventoryService;
        UserService userService;
        BusinessService businessService;
        NotificationService notificationService;

        public AddSupplierItemViewModel(
            InventoryService inventoryService,
            UserService userService,
            BusinessService businessService,
            NotificationService notificationService)
        {
            this.inventoryService = inventoryService;
            this.userService = userService;
            this.businessService = businessService;
            this.notificationService = notificationService;

            // BarcodeId = "4807770270017";
            // WItemName = "Lucky Me Beef Noodles";
            // WItemDesc = "Beef flavored instant mami noodle soup";
            // WItemType = "Instant Noodle";
            // WRetailPrice = 10;
            // WDelivQuantity = 10;
            // WUnitCost = 8.75f;
        }



        [RelayCommand]
        async Task SaveChanges()
        {
            int emptyCount = 0;
            if (BarcodeId.Equals("")) emptyCount += 1;
            if (WItemName.Equals("")) emptyCount += 1;
            if (WItemDesc.Equals("")) emptyCount += 1;
            if (WItemType.Equals("")) emptyCount += 1;
            if (WRetailPrice == null) emptyCount += 1;
            if (WDelivQuantity == null) emptyCount += 1;
            if (WBatchCost == null && WUnitCost == null) emptyCount += 1;
            // if (WDateExpire == null) emptyCount += 1;
            if (Supplier.Type.Equals("Consignment") && WConsignmentSchedule.Equals("")) emptyCount += 1;
            if (Supplier.Type.Equals("Consignment") && WReturnByDate == null) emptyCount += 1;

            if (emptyCount > 0)
            {
                await Toast.Make("Make sure to fill out all the details", ToastDuration.Short, 14).Show();
                return;
            }

            // -- SAVE ITEM RECORD --
            var userInstance = await userService.GetUserInstanceAsync();
            var user = await userService.GetUserAsync(userInstance.UserId);
            // check if item already has a record
            var item = await inventoryService.GetItemAsync(BarcodeId);
            // if no record, just create a new one
            if (item is null)
            {
                item = new Item()
                {
                    Barcode = BarcodeId,
                    BusinessId = user.BusinessRefId,
                    Name = WItemName,
                    Type = WItemType,
                    RetailPrice = (float)WRetailPrice!
                };
                await inventoryService.AddItemAsync(item);
                // description
                var desc = new Description()
                {
                    ItemId = BarcodeId,
                    Text = WItemDesc,
                    LastModified = DateTime.Now.ToString()
                };
                await inventoryService.AddDescAsync(desc);
            }
            // else notify user that item description will be edited if there is any
            else
            {
                var desc = await inventoryService.GetItemDescAsync(item.Barcode);
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
                        await FillItemDetails(item.Barcode);
                        return;
                    }
                    else
                    {
                        if (editedFields.Contains("Description"))
                        {
                            desc.Text = WItemDesc;
                            await inventoryService.UpdateDescAsync(desc);
                        }
                        item.Name = WItemName;
                        item.Type = WItemType;
                        item.RetailPrice = (float)WRetailPrice!;
                        await inventoryService.UpdateItemAsync(item);
                    }
                }
            }


            // -- SAVE DELIVERY RECORD --
            if (WBatchCost == null || !IsBatchCost)
            {
                WBatchCost = WUnitCost * WDelivQuantity;
            }
            var _dateExpire = (DateTime)WDateExpire!;
            var delivery = new Delivery()
            {
                BusinessId = user.BusinessRefId,
                SupplierId = Supplier.Id,
                ItemBarcode = BarcodeId,
                DeliveryAmount = (int)WDelivQuantity!,
                DateDelivered = WDateDelivered.ToString("g"),
                ExpirationDate = _dateExpire.ToString("d"),
                BatchWorth = (float)WBatchCost!
            };
            if (Supplier.Type.ToLower().Equals("consignment"))
            {
                var _returnDate = (DateTime)WReturnByDate!;
                delivery.ConsignmentSchedule = WConsignmentSchedule;
                delivery.ReturnByDate = _returnDate.ToString("d");
            }
            await inventoryService.AddDeliveryAsync(delivery);


            // -- SAVE ITEM TO INVENTORY RECORD --
            var inv = new Inventory()
            {
                BusinessId = user.BusinessRefId,
                DeliveryId = delivery.Id,
                ItemBarcode = BarcodeId,
                TotalAmount = (int)WDelivQuantity!
            };
            await inventoryService.AddInventoryAsync(inv);


            // -- CREATE REMINDER FOR CONSIGNMENT AGREEMENT --
            if (Supplier.Type.ToLower().Equals("consignment"))
            {
                // create reminder
                var m = $"""
                The item {item.Name} delivered on {DateTime.Parse(delivery.DateDelivered):MM/dd/yyyy} is to be returned on {DateTime.Parse(delivery.ReturnByDate):MM/dd/yyyy}.
                Items Sold: {delivery.DeliveryAmount - inv.TotalAmount}
                Stock Remaining: {inv.TotalAmount} / {delivery.DeliveryAmount}
                Amount to Pay: {(delivery.DeliveryAmount - inv.TotalAmount) * (delivery.BatchWorth / delivery.DeliveryAmount):0.00}
                """;
                var r = new Reminder()
                {
                    BusinessId = user.BusinessRefId,
                    Type = ReminderType.ConsignmentDue,
                    Title = "Return Consignment Item",
                    Message = m,
                    NotifyAtDate = new DateTime(DateOnly.FromDateTime(DateTime.Parse(delivery.ReturnByDate)), new TimeOnly(9, 0)).ToString(),
                    RepeatDaily = false,
                    RelatedEntityId = delivery.Id,
                    IsEnabled = true,
                    Sent = false
                };
                // save to database
                await notificationService.AddReminderAsync(r);

                // schedule local notification
                DateTime date = DateTime.Parse(r.NotifyAtDate);
                // gentle reminder at 3pm before the actual due date
                date = new DateTime(DateOnly.FromDateTime(date).AddDays(-1), new TimeOnly(15, 0));
                await notificationService.ScheduleLocalNotification(
                    r.Id, r.Title, r.Message, date, r.Id.ToString()
                );
                // due date at 9 am
                date = new DateTime(DateOnly.FromDateTime(date).AddDays(1), new TimeOnly(9, 0));
                await notificationService.ScheduleLocalNotification(
                    r.Id, r.Title, r.Message, date, r.Id.ToString()
                );
            }


            // clear every entry and notify the user that delivery has been added
            BarcodeId = "";
            WItemName = "";
            WItemDesc = "";
            WItemType = "";
            WRetailPrice = null;
            WDelivQuantity = null;
            WUnitCost = null;
            WBatchCost = null;
            WConsignmentSchedule = "";
            string message = "item/s added to inventory";
            if (!IsFromInventory) message = "Delivery recorded and " + message;
            await Toast.Make(message, ToastDuration.Short, 14).Show();
        }


        [RelayCommand]
        async Task ShowScanner()
        {
            IsScannerVisible = true;
        }


        public void HideScanner()
        {
            IsScannerVisible = false;
        }


        public async Task<bool> FillItemDetails(string barcode)
        {
            BarcodeId = barcode;
            WDateDelivered = DateTime.Now;
            WDateExpire = DateTime.Now.AddYears(1);

            var item = await inventoryService.GetItemAsync(barcode);
            var desc = await inventoryService.GetItemDescAsync(barcode);
            if (item is null) return false;

            WItemName = item.Name;
            WItemDesc = desc.Text;
            WItemType = item.Type;
            WRetailPrice = item.RetailPrice;

            return true;
        }


        public async Task ClearItemDetails()
        {
            WItemName = null;
            WItemDesc = null;
            WItemType = null;
            WRetailPrice = null;
        }



        [RelayCommand]
        public void ToggleCost()
        {
            IsBatchCost = !IsBatchCost;
        }
    }
}
