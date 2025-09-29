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
        // [ObservableProperty]
        // string itemName;
        [ObservableProperty]
        bool suggestionsVisible = false;

        // private decimal totalPrice;
        // public decimal TotalPrice
        // {
        //     get => totalPrice;
        //     set
        //     {
        //         totalPrice = value;
        //         OnPropertyChanged();
        //         OnPropertyChanged(nameof(Change));
        //     }
        // }

        [ObservableProperty]
        float payment;
        // private decimal payment;
        // public decimal Payment
        // {
        //     get => payment;
        //     set
        //     {
        //         if (payment != value)
        //         {
        //             payment = value;
        //             OnPropertyChanged();
        //             OnPropertyChanged(nameof(Change));
        //         }
        //     }
        // }

        [ObservableProperty]
        public float change;

        // public event PropertyChangedEventHandler PropertyChanged;
        // protected void OnPropertyChanged([CallerMemberName] string name = "") =>
        //     PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        InventoryService inventoryService;
        SalesService salesService;

        [ObservableProperty]
        List<Item> allItems;
        [ObservableProperty]
        string barcodeId;
        [ObservableProperty]
        bool isBarcodeEntry = true;
        [ObservableProperty]
        bool isScannerVisible = false;


        private Transaction transactionUsingBarcode;


        public TransactionViewModel(InventoryService inventoryService, SalesService salesService)
        {
            Items = [];
            this.inventoryService = inventoryService;
            this.salesService = salesService;
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
        }


        private async Task InitItems()
        {
            if (AllItems is not null) return;
            AllItems = await inventoryService.GetAllItemsAsync();
        }


        public async Task SelectItem(string itemBarcode)
        {
            // ItemName = AllItems.Find(x => x.Barcode.Equals(itemBarcode))!.Name;
            SuggestionsVisible = false;
        }


        // [RelayCommand]
        // public async Task SuggesionLabelTapped()
        // {
        //     await InitItems();


        // }




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

                await Toast.Make("Item already added\nYou can change the quantity if you want to add more of this item", ToastDuration.Short, 14).Show();
                IsBusy = false;
                return;
            }

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
                bool hasEmpty = false;
                foreach (var item in Items)
                {
                    if (item.ItemName == "") { hasEmpty = true; break; }
                    if (item.Quantity < 0) { hasEmpty = true; break; }
                }
                if (Payment - TotalPrice <= 0) hasEmpty = true;

                if (hasEmpty)
                {
                    await Toast.Make("Make sure all fields are complete", ToastDuration.Short, 14).Show();
                    IsBusy = false;
                    return;
                }
            }

            // save process
            var salesTransaction = new SalesTransaction()
            {
                Date = DateTime.Today.ToString(),
                TotalPrice = TotalPrice,
                Payment = Payment,
                Change = Change
            };
            await salesService.AddSalesTransactionAsync(salesTransaction);

            foreach (var itemTransaction in Items)
            {
                var barcode = AllItems.Find(x => x.Name.Contains(itemTransaction.ItemName))!.Barcode;
                var sortedDeliveries = (await inventoryService.GetDeliveriesAsync(barcode)).OrderByDescending(x => DateTime.Parse(x.DateDelivered)).ToList();
                var invRecords = await inventoryService.GetInventoriesAsync(barcode);

                var salesItem = new SalesItem()
                {
                    TransactionId = salesTransaction.Id,
                    Name = itemTransaction.ItemName,
                    Barcode = barcode,
                    Quantity = itemTransaction.Quantity
                };
                await salesService.AddSalesItemAsync(salesItem);

                // subtraction of inventory
                int toSubtract = 0;
                foreach (var delivery in sortedDeliveries)
                {
                    var inv = invRecords.Find(x => x.DeliveryId == delivery.Id)!;
                    if (toSubtract > 0)
                    {
                        if (inv.TotalAmount - toSubtract < 0)
                        {
                            toSubtract -= inv.TotalAmount;
                            // remove that from inventory already
                            await inventoryService.DeleteInventory(inv);
                            continue;
                        }
                        else
                        {
                            inv.TotalAmount -= toSubtract;
                            await inventoryService.UpdateInventoryAsync(inv);
                            break;
                        }
                    }
                    else if (inv.TotalAmount - salesItem.Quantity < 0)
                    {
                        toSubtract = salesItem.Quantity - inv.TotalAmount;
                        // remove that from inventory already
                        await inventoryService.DeleteInventory(inv);
                        continue;
                    }
                    else
                    {
                        inv.TotalAmount -= salesItem.Quantity;
                        await inventoryService.UpdateInventoryAsync(inv);
                        break;
                    }
                }
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
            transactionUsingBarcode.SelectedIndex = AllItems.IndexOf(item);
            IsBarcodeEntry = false;

            return true;
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
        public void ToggleBarcode()
        {
            IsBarcodeEntry = !IsBarcodeEntry;
        }


        // public ICommand AddItemCommand => new Command(() =>
        // {
        //     AddItem(new Transaction());
        // });

        // public ICommand SaveTransactionCommand => new Command(async () =>
        // {
        //     if (Items.Count == 0)
        //     {
        //         var notsuccessPopup = new NoItemsPopup();
        //         Application.Current.MainPage.ShowPopup(notsuccessPopup);
        //         return;
        //     }

        //     // Create new record and copy Items
        //     var record = new TransactionRecord
        //     {
        //         Name = $"Transaction {TransactionStore.Records.Count + 1}",
        //         Date = DateTime.Now,
        //         TotalPrice = TotalPrice,
        //         Items = Items.Select(item => new Transaction
        //         {
        //             ItemName = item.ItemName,
        //             Quantity = item.Quantity,
        //             Price = item.Price
        //         }).ToList(),
        //         Payment = this.Payment,
        //         Change = this.Change
        //     };

        //     TransactionStore.Records.Add(record);

        //     // Show success popup
        //     var successPopup = new SaveInventoryPopup("Transaction saved!");
        //     Application.Current.MainPage.ShowPopup(successPopup);

        //     // Clear transaction form
        //     Items.Clear();
        //     TotalPrice = 0;
        //     Payment = 0;
        // });

    }
}
