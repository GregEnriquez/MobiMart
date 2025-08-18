namespace MobiMart.Api.Dtos;

public record class InventoryDto(
    int Id,
    int BusinessId,
    int DeliveryId,
    int DescriptionId,
    int TotalAmount,
    string ItemName,
    float RetailPrice,
    string ItemType
);


public record class CreateInventoryDto(
    int BusinessId,
    int DeliveryId,
    int DescriptionId,
    int TotalAmount,
    string ItemName,
    float RetailPrice,
    string ItemType
);


public record class UpdateInventoryDto(
    int TotalAmount,
    string ItemName,
    float RetailPrice,
    string ItemType
);
