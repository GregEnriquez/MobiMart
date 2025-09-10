using System;
using System.ComponentModel.DataAnnotations.Schema;
using SQLite;

namespace MobiMart.Model;

public class Supplier
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    [ForeignKey(nameof(Business))]
    public int BusinessId { get; set; } //fk
    public string Type { get; set; } = "";
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string Socials { get; set; } = "";
    public string Number { get; set; } = "";
    [ForeignKey(nameof(ContactInfo))]
    public int sContactId { get; set; }
    public required string LastModified { get; set; }
}
