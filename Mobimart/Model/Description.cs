using System;
using System.ComponentModel.DataAnnotations.Schema;
using SQLite;

namespace MobiMart.Model;

public class Description
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    [ForeignKey(nameof(Inventory))]
    public int ItemId { get; set; }
    public string Text { get; set; } = "";
    public required string LastModified { get; set; }
}
