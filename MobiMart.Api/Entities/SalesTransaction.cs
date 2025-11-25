using System;

namespace MobiMart.Api.Entities;

public class SalesTransaction : SyncEntity
{
    public Guid BusinessId { get; set; }
    
    public DateTimeOffset Date { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal Payment { get; set; }
    public decimal Change { get; set; }
}


public class SalesItem : SyncEntity
{
    public Guid TransactionId { get; set; } // links to SalesTransaction.Id
    
    public string Name { get; set; } = "";
    public string Barcode { get; set; } = "";
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}