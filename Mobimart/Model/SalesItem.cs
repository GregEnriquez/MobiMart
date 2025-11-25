using System;
using System.ComponentModel.DataAnnotations.Schema;
using SQLite;

namespace MobiMart.Model;

public class SalesItem : SyncEntity
{
    [Indexed]
    public Guid TransactionId { get; set; }

    public string Name { get; set; } = "";
    public string Barcode { get; set; } = "";
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
