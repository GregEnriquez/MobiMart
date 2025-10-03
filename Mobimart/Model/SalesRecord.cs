using System;

namespace MobiMart.Model;

public record SalesRecord
{
    public int TransactionId { get; set; }
    public string Date { get; set; } = "";
    public float TotalPrice { get; set; }
    public float Payment { get; set; }
    public float Change { get; set; }
    public List<SalesItem> Items { get; set; } = null!;
}
