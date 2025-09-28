using System;
using System.ComponentModel.DataAnnotations.Schema;
using SQLite;

namespace MobiMart.Model;

public class SalesItem
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    [ForeignKey(nameof(SalesTransaction))]
    public int TransactionId { get; set; }
    public string Name { get; set; } = "";
    public string Barcode { get; set; } = "";
    public int Quantity { get; set; }

}
