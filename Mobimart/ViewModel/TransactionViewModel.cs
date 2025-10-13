using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobiMart.Model;
using MobiMart.Service;
using MobiMart.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MobiMart.ViewModel
{
    public partial class TransactionViewModel : BaseViewModel
    {
        // [ObservableProperty]
        // List<Transaction> items;
        public ObservableCollection<Transaction> Items { get; set; } = new ObservableCollection<Transaction>();
        public ObservableCollection<Item> FilteredItemSuggestions { get; set; }

        [ObservableProperty]
        float totalPrice;
        [ObservableProperty]
        bool suggestionsVisible = false;

        [ObservableProperty]
        float payment;
        [ObservableProperty]
        public float change;

        [ObservableProperty]
        List<Item> allItems;
        [ObservableProperty]
        string barcodeId;
        [ObservableProperty]
        bool isBarcodeEntry = true;
        [ObservableProperty]
        bool isScannerVisible = false;

        InventoryService inventoryService;
        SalesService salesService;
        NotificationService notificationService;

        private Transaction transactionUsingBarcode;


        public TransactionViewModel(InventoryService inventoryService, SalesService salesService, NotificationService notificationService)
        {
            Items = [];
            this.inventoryService = inventoryService;
            this.salesService = salesService;
            this.notificationService = notificationService;
        }


        [RelayCommand]
        public void AddItem()
        {
            var item = new Transaction();
            item.OnQuantityOrPriceChanged = UpdateTotal;
            Items.Add(item);
            UpdateTotal();
        }

        private void UpdateTotal()
        {
            TotalPrice = 0;
            foreach (var item in Items)
            {
                TotalPrice += item.Price * item.Quantity;
            }
            UpdateChange();
        }


        private async Task InitItems()
        {
            // if (AllItems is not null) return;
            AllItems = await inventoryService.GetAllItemsAsync();
        }


        public async Task SelectItem(string itemBarcode)
        {
            // ItemName = AllItems.Find(x => x.Barcode.Equals(itemBarcode))!.Name;
            SuggestionsVisible = false;
        }


        public async Task OnEntryTextChanged(string input)
        {
            await InitItems();
            var filtered = new List<Item>();


            if (string.IsNullOrWhiteSpace(input))
            {
                SuggestionsVisible = false;
                FilteredItemSuggestions.Clear();
                return;
            }

            if (input.Contains(' '))
            {
                filtered = AllItems.Where(x => x.Name.Contains(input)).ToList();
            }
            else if (char.IsDigit(input[0]))
            {
                filtered = AllItems.Where(x => x.Barcode.Contains(input)).ToList();
            }
            else
            {
                filtered = AllItems.Where(x => x.Name.Contains(input)).ToList();
            }


            FilteredItemSuggestions = new ObservableCollection<Item>(filtered);
            SuggestionsVisible = filtered.Any();
        }


        public async Task OnAppearing()
        {
            await InitItems();
        }


        public void UpdateChange()
        {
            if (Payment - TotalPrice <= 0)
            {
                Change = 0;
                return;
            }
            Change = Payment - TotalPrice;
        }



        public async void OnItemSelected(Picker picker, Transaction transactionItem)
        {
            if (IsBusy) return;
            IsBusy = true;
            var idx = Items.IndexOf(transactionItem);
            var item = AllItems[picker.SelectedIndex];
            var transaction = Items[idx];

            if (transaction.Quantity < 1) transaction.Quantity = 1;
            transaction.ItemName = item.Name;
            transaction.Price = item.RetailPrice;

            foreach (var existingTransaction in Items)
            {
                if (Object.ReferenceEquals(transaction, existingTransaction)) continue;
                if (!transaction.ItemName.Equals(existingTransaction.ItemName)) continue;

                transaction.Quantity = 0;
                transaction.ItemName = "";
                transaction.Price = 0;
                picker.SelectedIndex = -1;
                picker.SelectedItem = null;

                await Toast.Make("Item already added\nYou can change the quantity if you want to add more of this item", ToastDuration.Short, 14).Show();
                IsBusy = false;
                return;
            }

            transactionItem.IsBarcodeEntry = false;

            IsBusy = false;
        }


        [RelayCommand]
        public void DeleteTransaction(Transaction transactionItem)
        {
            if (IsBusy) return;
            IsBusy = true;
            Items.Remove(transactionItem);
            UpdateTotal();
            IsBusy = false;
        }


        [RelayCommand]
        public async Task SaveTransaction()
        {
            if (IsBusy) return;
            IsBusy = true;

            // input validation
            if (Items.Count < 1)
            {
                await Toast.Make("You can't save nothing", ToastDuration.Short, 14).Show();
                IsBusy = false;
                return;
            }
            else if (Items.Count > 0)
            {
                if (Payment - TotalPrice < 0)
                {
                    await Toast.Make("Payment entered is insufficient", ToastDuration.Short, 14).Show();
                    IsBusy = false;
                    return;
                }

                bool hasEmpty = false;
                foreach (var item in Items)
                {
                    if (item.ItemName is null) { hasEmpty = true; break; }
                    if (item.Quantity <= 0) { hasEmpty = true; break; }
                }

                if (hasEmpty)
                {
                    await Toast.Make("Make sure all fields are complete", ToastDuration.Short, 14).Show();
                    IsBusy = false;
                    return;
                }
            }


            // save process
            var businessId = -1;
            if (Shell.Current.BindingContext is FlyoutMenuViewModel vm)
            {
                businessId = vm.BusinessId;
            }
            bool salesTransactionSaved = false;
            var salesTransaction = new SalesTransaction()
            {
                BusinessId = businessId,
                Date = DateTime.Now.ToString("g"),
                TotalPrice = TotalPrice,
                Payment = Payment,
                Change = Change
            };

            foreach (var itemTransaction in Items)
            {
                var barcode = AllItems.Find(x => x.Name.Contains(itemTransaction.ItemName))!.Barcode;
                var sortedDeliveries = (await inventoryService.GetDeliveriesAsync(barcode)).OrderBy(x => DateTime.Parse(x.DateDelivered)).ToList();
                var invRecords = await inventoryService.GetInventoriesAsync(barcode);

                // input validation part 2: check if each item transaction has enough stock in the inventory
                int totalInv = 0;
                foreach (var inv in invRecords)
                {
                    totalInv += inv.TotalAmount;
                }
                if (totalInv < itemTransaction.Quantity)
                {
                    await Toast.Make($"Not enough stock for item {itemTransaction.ItemName}. Remaining: {totalInv}").Show();
                    IsBusy = false;
                    return;
                }

                // saving of sales transaction
                if (!salesTransactionSaved)
                {
                    await salesService.AddSalesTransactionAsync(salesTransaction);
                    salesTransactionSaved = true;
                }
                var salesItem = new SalesItem()
                {
                    TransactionId = salesTransaction.Id,
                    Name = itemTransaction.ItemName,
                    Barcode = barcode,
                    Quantity = itemTransaction.Quantity,
                    Price = itemTransaction.Price * itemTransaction.Quantity
                };
                await salesService.AddSalesItemAsync(salesItem);

                // subtraction of inventory
                int toSubtract = 0;
                foreach (var delivery in sortedDeliveries)
                {
                    var inv = invRecords.Find(x => x.DeliveryId == delivery.Id)!;
                    if (inv is null) continue; // inventory for that delivery record is already emptied(ubos na)

                    if (toSubtract > 0)
                    {
                        if (inv.TotalAmount - toSubtract < 0)
                        {
                            toSubtract -= inv.TotalAmount;
                            // remove that from inventory already
                            invRecords.Remove(inv);
                            await inventoryService.DeleteInventory(inv);
                            continue;
                        }
                        else
                        {
                            inv.TotalAmount -= toSubtract;
                            if (inv.TotalAmount == 0)
                                await inventoryService.DeleteInventory(inv);
                            else
                                await inventoryService.UpdateInventoryAsync(inv);
                            break;
                        }
                    }
                    else if (inv.TotalAmount - salesItem.Quantity < 0)
                    {
                        toSubtract = salesItem.Quantity - inv.TotalAmount;
                        // remove that from inventory already
                        invRecords.Remove(inv);
                        await inventoryService.DeleteInventory(inv);
                        continue;
                    }
                    else
                    {
                        inv.TotalAmount -= salesItem.Quantity;
                        if (inv.TotalAmount == 0)
                            await inventoryService.DeleteInventory(inv);
                        else
                            await inventoryService.UpdateInventoryAsync(inv);
                        break;
                    }
                }

                // check if needed reminder
                totalInv = 0;
                foreach (var inv in invRecords)
                {
                    totalInv += inv.TotalAmount;
                }

                if (totalInv >= 10) // low stock threshold
                {
                    continue;
                }


                // create reminder
                var r = new Reminder()
                {
                    BusinessId = businessId,
                    Type = ReminderType.SupplyRunout,
                    Title = "Stock Running Low",
                    Message = $"Stock for item {itemTransaction.ItemName} is running low\nRemaining Stock: {totalInv}",
                    NotifyAtDate = DateTime.Now.ToString(),
                    RepeatDaily = true,
                    RelatedEntityId = invRecords[0].Id,
                    IsEnabled = true,
                    Sent = false
                };
                // save to database
                await notificationService.AddReminderAsync(r);

                // schedule local notification
                DateTime date = DateTime.Parse(r.NotifyAtDate);
                date = date.AddSeconds(5);
                await notificationService.ScheduleLocalNotification(
                    r.Id, r.Title, r.Message, date, r.Id.ToString()
                );

                IsBusy = false;
            }


            // reset page
            TotalPrice = 0;
            Payment = 0;
            Change = 0;
            Items.Clear();
            await Toast.Make("Sales Transaction Succesfully Saved", ToastDuration.Short, 14).Show();

            IsBusy = false;
        }


        public bool PickItem(string barcode)
        {
            var item = AllItems.Find(x => x.Barcode.Equals(barcode));
            if (item is null) return false;

            transactionUsingBarcode.BarcodeId = barcode;
            transactionUsingBarcode.SelectedItem = item;

            return true;
        }


        public bool PickItem(string barcode, Transaction transactionItem)
        {
            transactionUsingBarcode = transactionItem;
            return PickItem(barcode);
        }


        [RelayCommand]
        public void ShowScanner(Transaction transactionItem)
        {
            IsScannerVisible = true;
            transactionUsingBarcode = transactionItem;
        }


        [RelayCommand]
        public void HideScanner()
        {
            IsScannerVisible = false;
            transactionUsingBarcode = null;
        }


        [RelayCommand]
        public void ToggleBarcode(Transaction transactionItem)
        {
            transactionItem.IsBarcodeEntry = !transactionItem.IsBarcodeEntry;
        }
    }
}
