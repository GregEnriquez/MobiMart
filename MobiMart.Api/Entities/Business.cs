using System;

namespace MobiMart.Api.Entities;

public class Business
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required string Code { get; set; }
}
