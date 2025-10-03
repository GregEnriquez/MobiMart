using System;
using System.ComponentModel.DataAnnotations.Schema;
using SQLite;

namespace MobiMart.Model;

public class Delivery
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; } // deliveryId
    [ForeignKey(nameof(Supplier))]
    public int SupplierId { get; set; }
    [ForeignKey(nameof(Item))]
    public string ItemBarcode { get; set; } = "";
    [ForeignKey(nameof(Business))]
    public int BusinessId { get; set; }
    public int DeliveryAmount { get; set; }
    public string DateDelivered { get; set; } = "";
    public string ExpirationDate { get; set; } = "";
    public float BatchWorth { get; set; }
    public string ConsignmentSchedule { get; set; } = "";
    public string ReturnByDate { get; set; } = "";
}
