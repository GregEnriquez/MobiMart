using MobiMart.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobiMart.ViewModel
{
    public class SupplierInventoryViewModel
    {
        public ObservableCollection<WholesaleInventory> supplierItems { get; set; }

        public SupplierInventoryViewModel()
        {

            supplierItems = new ObservableCollection<WholesaleInventory>
            {
                new WholesaleInventory {
                    deliveryId = 1,
                    wItemName = "Zesto Apple 50ml",
                    wDelivQuantity = 50,
                    wDateDelivered = DateTime.Now,
                    wDateExpire = DateTime.Now.AddMonths(6),
                    wBatchWorth = 1000,
                    wItemType = "Drink",
                    wItemDesc = "Fruit Juice"
                }
            };
        }
    }
}
