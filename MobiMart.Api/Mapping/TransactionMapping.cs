using System;
using MobiMart.Api.Dtos;
using MobiMart.Api.Entities;

namespace MobiMart.Api.Mapping;

public static class TransactionMapping
{
    public static SalesTransactionDto ToDto(this SalesTransaction entity)
    {
        return new SalesTransactionDto(
            entity.Id,
            entity.BusinessId,
            entity.Date,
            entity.TotalPrice,
            entity.Payment,
            entity.Change,
            entity.LastUpdatedAt,
            entity.IsDeleted
        );
    }

    public static SalesTransaction ToEntity(this CreateSalesTransactionDto dto)
    {
        return new SalesTransaction
        {
            Id = dto.Id,
            BusinessId = dto.BusinessId,
            Date = dto.Date,
            TotalPrice = dto.TotalPrice,
            Payment = dto.Payment,
            Change = dto.Change,
            LastUpdatedAt = DateTimeOffset.UtcNow,
            IsDeleted = dto.IsDeleted
        };
    }

    public static SalesItemDto ToDto(this SalesItem entity)
    {
        return new SalesItemDto(
            entity.Id,
            entity.TransactionId,
            entity.Name,
            entity.Barcode,
            entity.Price,
            entity.Quantity,
            entity.LastUpdatedAt,
            entity.IsDeleted
        );
    }

    public static SalesItem ToEntity(this CreateSalesItemDto dto)
    {
        return new SalesItem
        {
            Id = dto.Id,
            TransactionId = dto.TransactionId,
            Name = dto.Name,
            Barcode = dto.Barcode,
            Price = dto.Price,
            Quantity = dto.Quantity,
            LastUpdatedAt = DateTimeOffset.UtcNow,
            IsDeleted = dto.IsDeleted
        };
    }
}
