using System;

namespace MobiMart.Api.Entities;

public class WholesaleInventory : SyncEntity
{
    public Guid SupplierId { get; set; }
    public Guid DeliveryId { get; set; }

    public string WItemName { get; set; } = "";
    public string WItemDesc { get; set; } = "";
    public string WItemType { get; set; } = "";
    
    public int WDelivQuantity { get; set; }
    public DateTimeOffset WDateDelivered { get; set; }
    public DateTimeOffset WDateExpire { get; set; }
    public decimal WBatchWorth { get; set; } 
}
