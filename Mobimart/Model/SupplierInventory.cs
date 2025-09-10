using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobiMart.Model
{
    public class WholesaleInventory
    {
        [PrimaryKey, AutoIncrement]
        public int deliveryId { get; set; }
        [ForeignKey(nameof(Supplier))]
        public int supplierId { get; set; } //fk
        public string wItemName { get; set; } = "";
        public int wDelivQuantity { get; set; }
        public DateTime wDateDelivered { get; set; }
        public DateTime wDateExpire { get; set; }
        public decimal wBatchWorth { get; set; }
        public string wItemDesc { get; set; } = "";
        public string wItemType { get; set; } = "";

    }

    public class ConsignmentInventory
    {
        [PrimaryKey, AutoIncrement]
        public int deliveryId { get; set; }
        [ForeignKey(nameof(Supplier))]
        public int supplierId { get; set; } //fk
        public string cItemName { get; set; } = "";
        public int cDelivQuantity { get; set; }
        public string consignmentSched { get; set; } = "";
        public DateTime cDateDelivered { get; set; }
        public DateTime cDateExpire { get; set; }
        public decimal cBatchWorth { get; set; }
        public string cItemDesc { get; set; } = "";
        public string cItemType { get; set; } = "";
    }
}
