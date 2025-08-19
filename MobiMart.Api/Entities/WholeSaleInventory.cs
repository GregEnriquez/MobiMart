using System;

namespace MobiMart.Api.Entities;

public class WholeSaleInventory
{
    public int Id { get; set; } // deliveryId
    public int SupplierId { get; set; }
    public string ItemName { get; set; } = "";
    public int DeliveryAmount { get; set; }
    public DateOnly DateDelivered { get; set; }
    public DateOnly ExpirationDate { get; set; }
    public float BatchWorth { get; set; }
    public string ItemType { get; set; } = "";
}
