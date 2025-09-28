using System;

namespace MobiMart.Model;

public record InventoryRecord
{
    public int Id { get; set; }
    public string Barcode { get; set; } = "";
    public string Name { get; set; } = "";
    public int Quantity { get; set; }
    public float Price { get; set; }
    public string Type { get; set; } = "";
    public string Description { get; set; } = "";
}
