using System;
using System.ComponentModel.DataAnnotations.Schema;
using SQLite;

namespace MobiMart.Model;

public class WholeSaleInventory
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; } // deliveryId
    [ForeignKey(nameof(Supplier))]
    public int SupplierId { get; set; }
    public string ItemName { get; set; } = "";
    public int DeliveryAmount { get; set; }
    public string DateDelivered { get; set; } = "";
    public string ExpirationDate { get; set; } = "";
    public float BatchWorth { get; set; }
    public string ItemType { get; set; } = "";
}
