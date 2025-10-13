using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MobiMart.Model;
using Microsoft.Maui.Controls;
using MobiMart.View;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;
using MobiMart.Service;
using CommunityToolkit.Maui.Extensions;

namespace MobiMart.ViewModel
{
    public partial class InventoryViewModel : BaseViewModel
    {

        // public ObservableCollection<Inventory> InventoryItems { get; set; }
        [ObservableProperty]
        List<InventoryRecord> inventoryItems;

        [ObservableProperty]
        string searchText;

        private List<InventoryRecord> _allItems;
        InventoryService inventoryService;

        // private ObservableCollection<Inventory> _allItems; // original list

        // private string _searchText;
        // public string SearchText
        // {
        //     get => _searchText;
        //     set
        //     {
        //         _searchText = value;
        //         OnPropertyChanged();
        //         FilterItems(_searchText);
        //     }
        // }

        // public ICommand EditCommand { get; set; }

        public InventoryViewModel(InventoryService inventoryService)
        {
            this.inventoryService = inventoryService;
            // InventoryItems = new ObservableCollection<Inventory>();
            // _allItems = new ObservableCollection<Inventory>();

            // //_allItems.Add(newItemFromDb);
            // //InventoryItems.Add(newItemFromDb);

            // // sample data (for testing UI)
            // _allItems.Add(new Inventory
            // {
            //     Id = 1,
            //     ItemName = "Milk",
            //     TotalAmount = 20,
            //     RetailPrice = 50,
            //     ItemType = "Dairy",
            //     LastModified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            // });

            // _allItems.Add(new Inventory
            // {
            //     Id = 2,
            //     ItemName = "Bread",
            //     TotalAmount = 15,
            //     RetailPrice = 30,
            //     ItemType = "Bakery",
            //     LastModified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            // });

            // foreach (var item in _allItems)
            //     InventoryItems.Add(item);
            // // command for adding item
            // AddItemCommand = new Command(AddItem);
            // EditCommand = new Command<Inventory>(async (item) => await EditItem(item));
        }


        [RelayCommand]
        public async Task AddItem()
        {
            if (IsBusy) return;
            IsBusy = true;

            var navParameter = new Dictionary<string, object>()
            {
                {"IsFromInventory", true}
            };
            try
            {
                await Shell.Current.GoToAsync($"{nameof(SupplierList)}?IsFromInventory={true}");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
            IsBusy = false;
        }


        [RelayCommand]
        public async Task Edit(string barcode)
        {
            var param = new Dictionary<string, object>
            {
                {"ItemBarcode", barcode}
            };
            try
            {
                var popup = new EditInventoryPopup(new EditInventoryPopupViewModel(inventoryService, barcode));
                await Shell.Current.ShowPopupAsync(popup);
            }
            catch (Exception e)
            {
                
            }
        }


        public async Task RefreshInventoryRecords()
        {
            if (IsBusy) return;
            IsBusy = true;

            InventoryItems = [];
            _allItems = await inventoryService.GetInventoryRecordsAsync();
            InventoryItems = [.. _allItems];

            IsBusy = false;
        }

        public async Task FilterItems(string query)
        {
            InventoryItems = [];

            InventoryItems = [.. _allItems.Where(i => i.Name.ToLower().Contains(query.ToLower()))];

            // var filtered = string.IsNullOrWhiteSpace(query)
            //     ? _allItems
            //     : _allItems.Where(i => i.ItemName.ToLower().Contains(query.ToLower()));

            // foreach (var item in filtered)
            //     InventoryItems.Add(item);
        }


        [RelayCommand]
        public async Task ScanBarcode()
        {
            var param = new Dictionary<string, object>
            {
                {"IsFromInventory", true},
                {"IsFromScanBarcode", true}
            };

            await Shell.Current.GoToAsync(nameof(SupplierList), true, param);
        }
        

        // private void AddItem()
        // {
        //     var newItem = new Inventory
        //     {
        //         Id = _allItems.Count + 1,

        //         LastModified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        //     };

        //     _allItems.Add(newItem);
        //     InventoryItems.Add(newItem);
        // }

        // private async Task EditItem(Inventory item)
        // {
        //     if (item == null) return;
        //     await Application.Current.MainPage.Navigation.PushAsync(new EditInventory(item));
        // }
    }
}
