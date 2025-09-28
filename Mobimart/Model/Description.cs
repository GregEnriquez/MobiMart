using System;
using System.ComponentModel.DataAnnotations.Schema;
using SQLite;

namespace MobiMart.Model;

public class Description
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    [ForeignKey(nameof(Item))]
    public string ItemId { get; set; } = "";
    public string Text { get; set; } = "";
    public string LastModified { get; set; } = "";
}
