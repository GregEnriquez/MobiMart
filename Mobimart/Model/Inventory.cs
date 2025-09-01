using System;
using System.ComponentModel.DataAnnotations.Schema;
using SQLite;

namespace MobiMart.Model;

public class Inventory
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    [ForeignKey(nameof(Business))]
    public int BusinessId { get; set; }
    [ForeignKey(nameof(WholeSaleInventory))]
    public int DeliveryId { get; set; } // refers to wholesale inventory
    [ForeignKey(nameof(Description))]
    public int DescriptionId { get; set; } // DEV NOTE: I don't think this is supposed to be here (I just copied whats on the ERD)
    public int TotalAmount { get; set; }
    public string ItemName { get; set; } = "";
    public float RetailPrice { get; set; }
    public string ItemType { get; set; } = "";
    public required string LastModified { get; set; }
}
