using System;
using MobiMart.Api.Dtos;
using MobiMart.Api.Entities;

namespace MobiMart.Api.Mapping;

public static class CompletedContractMapping
{
    public static CompletedContractDto ToDto(this CompletedContract entity)
    {
        return new CompletedContractDto(
            entity.Id,
            entity.BusinessId,
            entity.SupplierName,
            entity.ReturnDate,
            entity.DateReturned,
            entity.AmountToPay,
            entity.ProofImageData,
            entity.LastUpdatedAt,
            entity.IsDeleted
        );
    }


    public static CompletedContract ToEntity(this CreateCompletedContractDto dto)
    {
        return new CompletedContract
        {
            Id = dto.Id,
            LastUpdatedAt = DateTimeOffset.UtcNow,
            IsDeleted = dto.IsDeleted,
            BusinessId = dto.BusinessId,
            SupplierName = dto.SupplierName,
            ReturnDate = dto.ReturnDate,
            DateReturned = dto.DateReturned,
            AmountToPay = dto.AmountToPay,
            ProofImageData = dto.ProofImageData
        };
    }


    public static CompletedContractItemDto ToDto(this CompletedContractItem entity)
    {
        return new CompletedContractItemDto(
            entity.Id,
            entity.ContractId,
            entity.Name,
            entity.SoldQuantity,
            entity.ReturnQuantity,
            entity.LastUpdatedAt
        );
    }


    public static CompletedContractItem ToEntity(this CreateCompletedContractItemDto dto)
    {
        return new CompletedContractItem
        {
            Id = dto.Id,
            LastUpdatedAt = DateTimeOffset.UtcNow,
            IsDeleted = dto.IsDeleted,
            ContractId = dto.ContractId,
            Name = dto.Name,
            SoldQuantity = dto.SoldQuantity,
            ReturnQuantity = dto.ReturnQuantity,
        };
    }
}
