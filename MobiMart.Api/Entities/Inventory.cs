using System;

namespace MobiMart.Api.Entities;

public class Inventory : SyncEntity
{
    public Guid BusinessId { get; set; }
    public Guid DeliveryId { get; set; }
    public string ItemBarcode { get; set; } = "";
    public int TotalAmount { get; set; }
    
}
