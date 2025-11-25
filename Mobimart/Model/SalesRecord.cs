using System;

namespace MobiMart.Model;

public record SalesRecord
{
    public Guid TransactionId { get; set; }
    public DateTime Date { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal Payment { get; set; }
    public decimal Change { get; set; }
    public List<SalesItem> Items { get; set; } = null!;
}
