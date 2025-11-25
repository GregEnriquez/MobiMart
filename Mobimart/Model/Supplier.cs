using System;
using System.ComponentModel.DataAnnotations.Schema;
using SQLite;

namespace MobiMart.Model;

public class Supplier : SyncEntity
{
    [Indexed]
    public Guid BusinessId { get; set; } //fk
    
    public string Type { get; set; } = "";
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string Socials { get; set; } = "";
    public string Number { get; set; } = "";
}
