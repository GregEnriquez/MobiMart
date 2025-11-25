using System;

namespace MobiMart.Api.Entities;

public class Supplier : SyncEntity
{
    public Guid BusinessId { get; set; } //fk

    public string Type { get; set; } = "";
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string Socials { get; set; } = "";
    public string Number { get; set; } = "";
}
