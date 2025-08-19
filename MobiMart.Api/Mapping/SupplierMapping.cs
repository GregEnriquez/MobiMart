using System;
using MobiMart.Api.Dtos;
using MobiMart.Api.Entities;

namespace MobiMart.Api.Mapping;

public static class SupplierMapping
{
    public static Supplier ToEntity(this CreateSupplierDto newSupplier)
    {
        return new Supplier()
        {
            BusinessId = newSupplier.BusinessId,
            Type = newSupplier.Type,
            Name = newSupplier.Name,
            Email = newSupplier.Email,
        };
    }


    public static Supplier ToEntity(this UpdateSupplierDto updatedSupplier, int id, int businessId, Socials socials)
    {
        return new Supplier()
        {
            Id = id,
            BusinessId = businessId,
            Type = updatedSupplier.Type,
            Name = updatedSupplier.Name,
            Email = updatedSupplier.Email,
            SocialsId = socials.Id,
            Socials = socials
        };
    }


    public static SupplierDto ToDto(this Supplier supplier, Socials socials)
    {
        return new SupplierDto(
            supplier.Id,
            supplier.BusinessId,
            supplier.Type,
            supplier.Name,
            supplier.Email,
            socials.Link,
            socials.Description
        );
    }
}
