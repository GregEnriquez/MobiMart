using System;

namespace MobiMart.Api.Dtos;

public record ItemDto(
    Guid Id,
    Guid BusinessId,
    string Barcode,
    string Name,
    string Type,
    decimal RetailPrice,
    Guid DescriptionId,
    DateTimeOffset LastUpdatedAt,
    bool IsDeleted
);

public record CreateItemDto(
    Guid Id,
    Guid BusinessId,
    string Barcode,
    string Name,
    string Type,
    decimal RetailPrice,
    Guid DescriptionId,
    bool IsDeleted
);
