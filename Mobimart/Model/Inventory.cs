using SQLite;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace MobiMart.Model;

public class Inventory : SyncEntity
{
    [Indexed]
    public Guid BusinessId { get; set; }
    [Indexed]
    public Guid DeliveryId { get; set; }
    [Indexed]
    public string ItemBarcode { get; set; } = "";
    public int TotalAmount { get; set; }

    // public string ItemType { get; set; } = "";
    // [ForeignKey(nameof(Description))]
    // public int DescriptionId { get; set; } // DEV NOTE: I don't think this is supposed to be here (I just copied whats on the ERD)
    // public string ItemName { get; set; } = "";
    // public float RetailPrice { get; set; }
}
