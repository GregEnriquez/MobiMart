namespace MobiMart.Model;

public record DeliveryRecord
{
    public Guid DeliveryId { get; set; }
    public string ItemName { get; set; } = "";
    public int DelivQuantity { get; set; }
    public DateTime DateDelivered { get; set; }
    public DateTime DateExpire { get; set; }
    public double BatchCostPrice { get; set; }
    public string ItemType { get; set; } = "";
    public string ItemDesc { get; set; } = "";
    public string Barcode { get; set; } = "";
    public int QuantityInStock { get; set; }

    public string ConsignmentSchedule { get; set; } = "";
    public DateTime? ReturnByDate { get; set; }
}
