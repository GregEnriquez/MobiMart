using System;

namespace MobiMart.Api.Entities;

public class Business
{
    public int Id { get; set; }
    public required string BusinessName { get; set; }
    public required string BusinessAddress { get; set; }
    public required string BusinessCode { get; set; }
}
