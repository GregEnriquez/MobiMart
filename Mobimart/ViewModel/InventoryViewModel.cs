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

        [ObservableProperty]
        bool isLoadingInventory = false;

        public InventoryViewModel(InventoryService inventoryService)
        {
            this.inventoryService = inventoryService;
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
            IsLoadingInventory = true;

            InventoryItems = [];
            _allItems = await inventoryService.GetInventoryRecordsAsync();
            InventoryItems = [.. _allItems];

            IsLoadingInventory = false;
            IsBusy = false;
        }

        public async Task FilterItems(string query)
        {
            InventoryItems = [];

            InventoryItems = [.. _allItems.Where(i => i.Name.ToLower().Contains(query.ToLower()))];
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
    }
}
