using System;
using System.ComponentModel.DataAnnotations.Schema;
using SQLite;

namespace MobiMart.Model;

public class SalesTransaction : SyncEntity
{
    [Indexed]
    public Guid BusinessId { get; set; }
    
    public DateTimeOffset Date { get; set; }

    public decimal TotalPrice { get; set; }
    public decimal Payment { get; set; }
    public decimal Change { get; set; }
}
