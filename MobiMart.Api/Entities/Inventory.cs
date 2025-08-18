using System;

namespace MobiMart.Api.Entities;

public class Inventory
{
    public int Id { get; set; }
    public int BusinessId { get; set; }
    public int DeliveryId { get; set; }
    public int DescriptionId { get; set; }
    public int TotalAmount { get; set; }
    public string ItemName { get; set; } = "";
    public float RetailPrice { get; set; }
    public string ItemType { get; set; } = "";
}
