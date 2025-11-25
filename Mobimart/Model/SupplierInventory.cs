using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobiMart.Model
{
    public class WholesaleInventory : SyncEntity
    {
        [Indexed]
        public Guid SupplierId { get; set; } //fk
        [Indexed]
        public Guid DeliveryId { get; set; } //fk
     
        public string ItemName { get; set; } = "";
        public string ItemDesc { get; set; } = "";
        public string ItemType { get; set; } = "";

        public DateTimeOffset DateDelivered { get; set; }
        public DateTimeOffset DateExpire { get; set; }

        public int DelivQuantity { get; set; }
        public decimal BatchWorth { get; set; }

    }

    public class ConsignmentInventory : SyncEntity
    {
        [Indexed]
        public int SupplierId { get; set; } //fk
        [Indexed]
        public int DeliveryId { get; set; } //fk

        public string ItemName { get; set; } = "";
        public string ItemDesc { get; set; } = "";
        public string ItemType { get; set; } = "";

        public string ConsignmentSched { get; set; } = "";
        public DateTimeOffset DateDelivered { get; set; }
        public DateTimeOffset DateExpire { get; set; }

        public int DelivQuantity { get; set; }
        public decimal BatchWorth { get; set; }
    }
}
