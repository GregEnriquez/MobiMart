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


    public static Supplier ToEntity(this UpdateSupplierDto updatedSupplier, Guid id, Guid businessId)
    {
        return new Supplier()
        {
            Id = id,
            LastUpdatedAt = DateTimeOffset.UtcNow,
            BusinessId = businessId,
            Type = updatedSupplier.Type,
            Name = updatedSupplier.Name,
            Email = updatedSupplier.Email,
            Socials = updatedSupplier.Socials,
            Number = updatedSupplier.Number,
        };
    }


    public static SupplierDto ToDto(this Supplier supplier)
    {
        return new SupplierDto(
            supplier.Id,
            supplier.BusinessId,
            supplier.Type,
            supplier.Name,
            supplier.Email,
            supplier.Socials,
            supplier.Number,
            supplier.LastUpdatedAt,
            supplier.IsDeleted
        );
    }
}
