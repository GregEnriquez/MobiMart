using System;
using System.ComponentModel.DataAnnotations.Schema;
using SQLite;

namespace MobiMart.Model;

public class Delivery : SyncEntity
{
    [Indexed]
    public Guid SupplierId { get; set; }
    [Indexed]
    public string ItemBarcode { get; set; } = "";
    [Indexed]
    public Guid BusinessId { get; set; }

    public int DeliveryAmount { get; set; }
    public decimal BatchWorth { get; set; }

    public DateTimeOffset DateDelivered { get; set; }
    public DateTimeOffset ExpirationDate { get; set; }

    // for consignment deliveries
    public string ConsignmentSchedule { get; set; } = "";
    public DateTimeOffset? ReturnByDate { get; set; }
}
