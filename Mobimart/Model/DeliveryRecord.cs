namespace MobiMart.Model;

public record DeliveryRecord
{
    public int DeliveryId { get; set; }
    public string ItemName { get; set; } = "";
    public int DelivQuantity { get; set; }
    public string DateDelivered { get; set; } = "";
    public string DateExpire { get; set; } = "";
    public double BatchCostPrice { get; set; }
    public string ItemType { get; set; } = "";
    public string ItemDesc { get; set; } = "";
    public string Barcode { get; set; } = "";
}
