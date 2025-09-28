using System;
using System.ComponentModel.DataAnnotations.Schema;
using SQLite;

namespace MobiMart.Model;

public class Item
{
    [PrimaryKey]
    public string Barcode { get; set; } = "";
    [ForeignKey(nameof(Business))]
    public int BusinessId { get; set; }
    [ForeignKey(nameof(Delivery))]
    public int DescriptionId { get; set; } // DEV NOTE: I don't think this is supposed to be here (I just copied whats on the ERD)
    public string Name { get; set; } = "";
    public string Type { get; set; } = "";
    public float RetailPrice { get; set; }
}
