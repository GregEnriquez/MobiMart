using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobiMart.Model;
using MobiMart.Service;
using MobiMart.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobiMart.ViewModel
{
    public partial class SupplierInventoryViewModel : BaseViewModel
    {
        // public ObservableCollection<WholesaleInventory> supplierItems { get; set; }
        [ObservableProperty]
        List<DeliveryRecord> supplierItems;

        [ObservableProperty]
        public Supplier supplier;


        InventoryService inventoryService;
        public SupplierInventoryViewModel(InventoryService inventoryService)
        {
            this.inventoryService = inventoryService;
            // supplierItems = new ObservableCollection<WholesaleInventory>
            // {
            //     new WholesaleInventory {
            //         deliveryId = 1,
            //         wItemName = "Zesto Apple 50ml",
            //         wDelivQuantity = 50,
            //         wDateDelivered = DateTime.Now,
            //         wDateExpire = DateTime.Now.AddMonths(6),
            //         wBatchWorth = 1000,
            //         wItemType = "Drink",
            //         wItemDesc = "Fruit Juice"
            //     }
            // };
        }


        [RelayCommand]
        public async Task AddTapped()
        {
            var navParam = new Dictionary<string, object>
            {
                {"Supplier", Supplier},
                {"IsFromInventory", false}
            };

            await Shell.Current.GoToAsync(nameof(AddSupplierItem), navParam);
        }



        public async Task RefreshRecords()
        {
            SupplierItems = [];
            SupplierItems = await inventoryService.GetDeliveryRecordsAsync(Supplier.Id);
        }
    }
}


// rename the EditSuppInventoryViewModel to edit inventory. then rename EditInventory to EditSupplierDeliveryViewMoedl ijoeqjrot 
