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
    [ForeignKey(nameof(Socials))]
    public int SocialsId { get; set; }
    public required string LastModified { get; set; }
}
