using System;

namespace MobiMart.Api.Entities;

public class Item : SyncEntity
{
    public Guid BusinessId { get; set; }

    public string Barcode { get; set; } = "";
    public string Name { get; set; } = "";
    public string Type { get; set; } = "";
    public decimal RetailPrice { get; set; }

    public Guid DescriptionId { get; set; }
}
