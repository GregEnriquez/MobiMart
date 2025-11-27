using System;

namespace MobiMart.Helpers.Dtos;

public record SalesTransactionDto(
    Guid Id,
    Guid BusinessId,
    DateTimeOffset Date,
    decimal TotalPrice,
    decimal Payment,
    decimal Change,
    DateTimeOffset LastUpdatedAt,
    bool IsDeleted
);

public record CreateSalesTransactionDto(
    Guid Id,
    Guid BusinessId,
    DateTimeOffset Date,
    decimal TotalPrice,
    decimal Payment,
    decimal Change,
    bool IsDeleted,
    DateTimeOffset LastUpdatedAt
);

public record SalesItemDto(
    Guid Id,
    Guid TransactionId,
    string Name,
    string Barcode,
    decimal Price,
    int Quantity,
    DateTimeOffset LastUpdatedAt,
    bool IsDeleted
);

public record CreateSalesItemDto(
    Guid Id,
    Guid TransactionId,
    string Name,
    string Barcode,
    decimal Price,
    int Quantity,
    bool IsDeleted,
    DateTimeOffset LastUpdatedAt
);
