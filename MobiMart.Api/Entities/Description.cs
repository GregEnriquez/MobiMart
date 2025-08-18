using System;

namespace MobiMart.Api.Entities;

public class Description
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public string Text { get; set; } = "";
}
