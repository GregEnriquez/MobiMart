using System;
using MobiMart.Helpers.Dtos;
using MobiMart.Model;

namespace MobiMart.Helpers.Mapping;

public static class SupplierMapping
{
    public static Supplier ToEntity(this CreateSupplierDto newSupplier)
    {
        return new Supplier()
        {
            Id = newSupplier.Id,
            LastUpdatedAt = newSupplier.LastUpdatedAt,
            IsDeleted = newSupplier.IsDeleted,
            BusinessId = newSupplier.BusinessId,
            Type = newSupplier.Type,
            Name = newSupplier.Name,
            Email = newSupplier.Email,
            Socials = newSupplier.Socials,
            Number = newSupplier.Number
        };
    }


    public static Supplier ToEntity(this SupplierDto newSupplier)
    {
        return new Supplier()
        {
            Id = newSupplier.Id,
            LastUpdatedAt = newSupplier.LastUpdatedAt,
            IsDeleted = newSupplier.IsDeleted,
            BusinessId = newSupplier.BusinessId,
            Type = newSupplier.Type,
            Name = newSupplier.Name,
            Email = newSupplier.Email,
            Socials = newSupplier.Socials,
            Number = newSupplier.Number
        };
    }


    public static CreateSupplierDto ToDto(this Supplier supplier)
    {
        return new CreateSupplierDto(
            supplier.Id,
            supplier.BusinessId,
            supplier.Type,
            supplier.Name,
            supplier.Email,
            supplier.Socials,
            supplier.Number,
            supplier.IsDeleted,
            supplier.LastUpdatedAt
        );
    }
}
