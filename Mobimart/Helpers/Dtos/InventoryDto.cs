namespace MobiMart.Helpers.Dtos;

public record InventoryDto(
    Guid Id,
    Guid BusinessId,
    Guid DeliveryId,
    string ItemBarcode,
    int TotalAmount,
    DateTimeOffset LastUpdatedAt,
    bool IsDeleted
);

public record CreateInventoryDto(
    Guid Id,
    Guid BusinessId,
    Guid DeliveryId,
    string ItemBarcode,
    int TotalAmount,
    bool IsDeleted,
    DateTimeOffset LastUpdatedAt
);


public record class UpdateInventoryDto(
    int TotalAmount
);
