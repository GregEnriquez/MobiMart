using System;
using System.ComponentModel.DataAnnotations.Schema;
using SQLite;

namespace MobiMart.Model;

public class Description : SyncEntity
{
    [Indexed]
    public Guid ItemId { get; set; }
    public string Text { get; set; } = "";
}
