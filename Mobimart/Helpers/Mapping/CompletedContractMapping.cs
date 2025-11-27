using System;
using MobiMart.Helpers.Dtos;
using MobiMart.Model;

namespace MobiMart.Helpers.Mapping;

public static class CompletedContractMapping
{
    public static CreateCompletedContractDto ToDto(this CompletedContract entity)
    {
        return new CreateCompletedContractDto(
            entity.Id,
            entity.BusinessId,
            entity.IsDeleted,
            entity.SupplierName,
            entity.ReturnDate,
            entity.DateReturned,
            entity.AmountToPay,
            entity.ProofImageData,
            entity.LastUpdatedAt
        );
    }


    public static CompletedContract ToEntity(this CreateCompletedContractDto dto)
    {
        return new CompletedContract
        {
            Id = dto.Id,
            LastUpdatedAt = dto.LastUpdatedAt,
            IsDeleted = dto.IsDeleted,
            BusinessId = dto.BusinessId,
            SupplierName = dto.SupplierName,
            ReturnDate = dto.ReturnDate,
            DateReturned = dto.DateReturned,
            AmountToPay = dto.AmountToPay,
            ProofImageData = dto.ProofImageData
        };
    }


    public static CompletedContract ToEntity(this CompletedContractDto dto)
    {
        return new CompletedContract
        {
            Id = dto.Id,
            LastUpdatedAt = dto.LastUpdatedAt,
            IsDeleted = dto.IsDeleted,
            BusinessId = dto.BusinessId,
            SupplierName = dto.SupplierName,
            ReturnDate = dto.ReturnDate,
            DateReturned = dto.DateReturned,
            AmountToPay = dto.AmountToPay,
            ProofImageData = dto.ProofImageData
        };
    }


    public static CreateCompletedContractItemDto ToDto(this CompletedContractItem entity)
    {
        return new CreateCompletedContractItemDto(
            entity.Id,
            entity.ContractId,
            entity.IsDeleted,
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
            LastUpdatedAt = dto.LastUpdatedAt,
            IsDeleted = dto.IsDeleted,
            ContractId = dto.ContractId,
            Name = dto.Name,
            SoldQuantity = dto.SoldQuantity,
            ReturnQuantity = dto.ReturnQuantity,
        };
    }


    public static CompletedContractItem ToEntity(this CompletedContractItemDto dto)
    {
        return new CompletedContractItem
        {
            Id = dto.Id,
            LastUpdatedAt = dto.LastUpdatedAt,
            IsDeleted = dto.IsDeleted,
            ContractId = dto.ContractId,
            Name = dto.Name,
            SoldQuantity = dto.SoldQuantity,
            ReturnQuantity = dto.ReturnQuantity,
        };
    }
}
