using System;

namespace MobiMart.Api.Entities;

public class Delivery : SyncEntity
{
    public Guid SupplierId { get; set; }
    public string ItemBarcode { get; set; } = "";
    public Guid BusinessId { get; set; }
    
    public int DeliveryAmount { get; set; } 
    public DateTimeOffset DateDelivered { get; set; }
    public DateTimeOffset ExpirationDate { get; set; }
    public decimal BatchWorth { get; set; }
    
    // for consignment deliveries
    public string ConsignmentSchedule { get; set; } = "";
    public DateTimeOffset? ReturnByDate { get; set; }
}
