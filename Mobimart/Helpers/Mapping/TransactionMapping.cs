using System;
using MobiMart.Helpers.Dtos;
using MobiMart.Model;

namespace MobiMart.Helpers.Mapping;

public static class TransactionMapping
{
    public static CreateSalesTransactionDto ToDto(this SalesTransaction entity)
    {
        return new CreateSalesTransactionDto(
            entity.Id,
            entity.BusinessId,
            entity.Date,
            entity.TotalPrice,
            entity.Payment,
            entity.Change,
            entity.IsDeleted,
            entity.LastUpdatedAt
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
            LastUpdatedAt = dto.LastUpdatedAt,
            IsDeleted = dto.IsDeleted
        };
    }


    public static SalesTransaction ToEntity(this SalesTransactionDto dto)
    {
        return new SalesTransaction
        {
            Id = dto.Id,
            BusinessId = dto.BusinessId,
            Date = dto.Date,
            TotalPrice = dto.TotalPrice,
            Payment = dto.Payment,
            Change = dto.Change,
            LastUpdatedAt = dto.LastUpdatedAt,
            IsDeleted = dto.IsDeleted
        };
    }


    public static CreateSalesItemDto ToDto(this SalesItem entity)
    {
        return new CreateSalesItemDto(
            entity.Id,
            entity.TransactionId,
            entity.Name,
            entity.Barcode,
            entity.Price,
            entity.Quantity,
            entity.IsDeleted,
            entity.LastUpdatedAt
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
            LastUpdatedAt = dto.LastUpdatedAt,
            IsDeleted = dto.IsDeleted
        };
    }


    public static SalesItem ToEntity(this SalesItemDto dto)
    {
        return new SalesItem
        {
            Id = dto.Id,
            TransactionId = dto.TransactionId,
            Name = dto.Name,
            Barcode = dto.Barcode,
            Price = dto.Price,
            Quantity = dto.Quantity,
            LastUpdatedAt = dto.LastUpdatedAt,
            IsDeleted = dto.IsDeleted
        };
    }
}
