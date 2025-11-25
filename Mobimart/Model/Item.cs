using System;
using System.ComponentModel.DataAnnotations.Schema;
using SQLite;

namespace MobiMart.Model;

public class Item : SyncEntity
{
    [Indexed]
    public Guid BusinessId { get; set; }
    [Indexed]
    public string Barcode { get; set; } = "";
    
    public string Name { get; set; } = "";
    public string Type { get; set; } = "";
    public decimal RetailPrice { get; set; }
    
    [Indexed]
    public Guid DescriptionId { get; set; } // DEV NOTE: I don't think this is supposed to be here (I just copied whats on the ERD)
}
