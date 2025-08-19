using System;

namespace MobiMart.Api.Entities;

public class SupplierContact
{
    public int Id { get; set; }
    public int SupplierId { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
}
