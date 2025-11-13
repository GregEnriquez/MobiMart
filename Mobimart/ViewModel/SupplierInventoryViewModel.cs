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
    
    public enum DeliveryCategory {Name, Quantity, DateDelivered, DateExpiry, BatchCost, ItemType, ConsignmentSched, DateReturn}

    public partial class SupplierInventoryViewModel : BaseViewModel
    {
        // public ObservableCollection<WholesaleInventory> supplierItems { get; set; }
        [ObservableProperty]
        List<DeliveryRecord> supplierItems;
        [ObservableProperty]
        public Supplier supplier;
        [ObservableProperty]
        int rotation = -90;
        [ObservableProperty]
        DeliveryCategory selectedCategory = DeliveryCategory.Quantity;
        [ObservableProperty] bool visibleCol1 = false;
        [ObservableProperty] bool visibleCol2 = true;
        [ObservableProperty] bool visibleCol3 = false;
        [ObservableProperty] bool visibleCol4 = false;
        [ObservableProperty] bool visibleCol5 = false;
        [ObservableProperty] bool visibleCol6 = false;
        [ObservableProperty] bool visibleCol7 = false;
        [ObservableProperty] bool visibleCol8 = false;

        List<DeliveryRecord> _supplierItems;
        bool isDescending = true;


        InventoryService inventoryService;
        public SupplierInventoryViewModel(InventoryService inventoryService)
        {
            this.inventoryService = inventoryService;
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


        [RelayCommand]
        public async Task CategoryTapped(string gridCol)
        {
            VisibleCol1 = false; VisibleCol2 = false; VisibleCol3 = false; VisibleCol4 = false;
            VisibleCol5 = false; VisibleCol6 = false; VisibleCol7 = false; VisibleCol8 = false;
            switch (gridCol)
            {
                case "1":
                    if (SelectedCategory == DeliveryCategory.Name) isDescending = !isDescending;
                    SelectedCategory = DeliveryCategory.Name;
                    VisibleCol1 = true;
                    break;
                case "2":
                    if (SelectedCategory == DeliveryCategory.Quantity) isDescending = !isDescending;
                    SelectedCategory = DeliveryCategory.Quantity;
                    VisibleCol2 = true;
                    break;
                case "3":
                    if (SelectedCategory == DeliveryCategory.DateDelivered) isDescending = !isDescending;
                    SelectedCategory = DeliveryCategory.DateDelivered;
                    VisibleCol3 = true;
                    break;
                case "4":
                    if (SelectedCategory == DeliveryCategory.DateExpiry) isDescending = !isDescending;
                    SelectedCategory = DeliveryCategory.DateExpiry;
                    VisibleCol4 = true;
                    break;
                case "5":
                    if (SelectedCategory == DeliveryCategory.BatchCost) isDescending = !isDescending;
                    SelectedCategory = DeliveryCategory.BatchCost;
                    VisibleCol5 = true;
                    break;
                case "6":
                    if (SelectedCategory == DeliveryCategory.ItemType) isDescending = !isDescending;
                    SelectedCategory = DeliveryCategory.ItemType;
                    VisibleCol6 = true;
                    break;
                case "7":
                    if (SelectedCategory == DeliveryCategory.ConsignmentSched) isDescending = !isDescending;
                    SelectedCategory = DeliveryCategory.ConsignmentSched;
                    VisibleCol7 = true;
                    break;
                case "8":
                    if (SelectedCategory == DeliveryCategory.DateReturn) isDescending = !isDescending;
                    SelectedCategory = DeliveryCategory.DateReturn;
                    VisibleCol8 = true;
                    break;
            }
            Rotation = isDescending ? -90 : 90;

            await RefreshRecords();
        }



        public async Task RefreshRecords()
        {
            IsBusy = true;
            _supplierItems = [];
            _supplierItems = await inventoryService.GetDeliveryRecordsAsync(Supplier.Id);
            await SortRecords();
            IsBusy = false;
        }



        private async Task SortRecords()
        {
            SupplierItems = [];
            switch (SelectedCategory)
            {
                case DeliveryCategory.Name:
                    SupplierItems = isDescending ? [.. _supplierItems.OrderByDescending(x => x.ItemName)] : [.. _supplierItems.OrderBy(x => x.ItemName)];
                    VisibleCol1 = true;
                    break;
                case DeliveryCategory.Quantity:
                    SupplierItems = isDescending ? [.. _supplierItems.OrderByDescending(x => x.QuantityInStock)] : [.. _supplierItems.OrderBy(x => x.QuantityInStock)];
                    VisibleCol2 = true;
                    break;
                case DeliveryCategory.DateDelivered:
                    SupplierItems = isDescending ? [.. _supplierItems.OrderByDescending(x => DateTime.Parse(x.DateDelivered))] : [.. _supplierItems.OrderBy(x => DateTime.Parse(x.DateDelivered))];
                    VisibleCol3 = true;
                    break;
                case DeliveryCategory.DateExpiry:
                    SupplierItems = isDescending ? [.. _supplierItems.OrderByDescending(x => DateTime.Parse(x.DateExpire))] : [.. _supplierItems.OrderBy(x => DateTime.Parse(x.DateExpire))];
                    VisibleCol4 = true;
                    break;
                case DeliveryCategory.BatchCost:
                    SupplierItems = isDescending ? [.. _supplierItems.OrderByDescending(x => x.BatchCostPrice)] : [.. _supplierItems.OrderBy(x => x.BatchCostPrice)];
                    VisibleCol5 = true;
                    break;
                case DeliveryCategory.ItemType:
                    SupplierItems = isDescending ? [.. _supplierItems.OrderByDescending(x => x.ItemType)] : [.. _supplierItems.OrderBy(x => x.ItemType)];
                    VisibleCol6 = true;
                    break;
                case DeliveryCategory.ConsignmentSched:
                    VisibleCol7 = true;
                    SupplierItems = isDescending ? [.. _supplierItems.OrderByDescending(x => x.ConsignmentSchedule)] : [.. _supplierItems.OrderBy(x => x.ConsignmentSchedule)];
                    break;
                case DeliveryCategory.DateReturn:
                    VisibleCol8 = true;
                    SupplierItems = isDescending ? [.. _supplierItems.OrderByDescending(x => DateTime.Parse(x.ReturnByDate))] : [.. _supplierItems.OrderBy(x => DateTime.Parse(x.ReturnByDate))];
                    break;
                default:
                    SupplierItems = [.. _supplierItems];
                    break;

            }
        }
    }
}


// rename the EditSuppInventoryViewModel to edit inventory. then rename EditInventory to EditSupplierDeliveryViewMoedl ijoeqjrot 
