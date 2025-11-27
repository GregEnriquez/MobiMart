using System;
using System.Runtime.CompilerServices;

namespace MobiMart.Helpers.Dtos;

public record CompletedContractDto(
    Guid Id,
    Guid BusinessId,
    string SupplierName,
    DateTimeOffset? ReturnDate,
    DateTimeOffset? DateReturned,
    decimal AmountToPay,
    byte[] ProofImageData,
    DateTimeOffset LastUpdatedAt,
    bool IsDeleted
);

public record CreateCompletedContractDto(
    Guid Id,
    Guid BusinessId,
    bool IsDeleted,
    string SupplierName,
    DateTimeOffset? ReturnDate,
    DateTimeOffset? DateReturned,
    decimal AmountToPay,
    byte[] ProofImageData,
    DateTimeOffset LastUpdatedAt
);

public record CompletedContractItemDto(
    Guid Id,
    Guid ContractId,
    string Name,
    int SoldQuantity,
    int ReturnQuantity,
    DateTimeOffset LastUpdatedAt,
    bool IsDeleted
);

public record CreateCompletedContractItemDto(
    Guid Id,
    Guid ContractId,
    bool IsDeleted,
    string Name,
    int SoldQuantity,
    int ReturnQuantity,
    DateTimeOffset LastUpdatedAt
);