namespace MobiMart.Api.Dtos;

public record class WholeSaleInventoryDto(
    int Id, // deliveryId
    int SupplierId,
    string ItemName,
    int DeliveryAmount,
    DateOnly DateDelivered,
    DateOnly ExpirationDate,
    float BatchWorth,
    string ItemType
);


public record class CreateWholeSaleInventoryDto(
    int SupplierId,
    string ItemName,
    int DeliveryAmount,
    DateOnly DateDelivered,
    DateOnly ExpirationDate,
    float BatchWorth,
    string ItemType
);


public record class UpdateWholeSaleInventoryDto(
    string ItemName,
    int DeliveryAmount,
    DateOnly DateDelivered,
    DateOnly ExpirationDate,
    float BatchWorth,
    string ItemType
);