using SQLite;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace MobiMart.Model;

public class Inventory 
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    [ForeignKey(nameof(Business))]
    public int BusinessId { get; set; }
    [ForeignKey(nameof(Delivery))]
    public int DeliveryId { get; set; }
    [ForeignKey(nameof(Item))]
    public string ItemBarcode { get; set; } = "";
    public int TotalAmount { get; set; }
    // public string ItemType { get; set; } = "";
    // [ForeignKey(nameof(Description))]
    // public int DescriptionId { get; set; } // DEV NOTE: I don't think this is supposed to be here (I just copied whats on the ERD)
    // public string ItemName { get; set; } = "";
    // public float RetailPrice { get; set; }
}
