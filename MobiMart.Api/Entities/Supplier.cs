using System;

namespace MobiMart.Api.Entities;

public class Supplier
{
    public int Id { get; set; }
    public int BusinessId { get; set; } //fk
    public string Type { get; set; } = "";
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public int SocialsId { get; set; }
    public Socials? Socials { get; set; }
}
