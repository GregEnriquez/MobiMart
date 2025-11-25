using System;

namespace MobiMart.Api.Entities;

public class Description : SyncEntity
{
    public Guid ItemId { get; set; }
    public string Text { get; set; } = "";
}
