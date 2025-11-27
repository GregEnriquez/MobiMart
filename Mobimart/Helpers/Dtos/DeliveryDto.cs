using System;

namespace MobiMart.Helpers.Dtos;

public record DeliveryDto(
    Guid Id,
    Guid SupplierId,
    Guid BusinessId,
    string ItemBarcode,
    int DeliveryAmount,
    DateTimeOffset DateDelivered,
    DateTimeOffset ExpirationDate,
    decimal BatchWorth,
    string ConsignmentSchedule,
    DateTimeOffset? ReturnByDate,
    DateTimeOffset LastUpdatedAt,
    bool IsDeleted
);

public record CreateDeliveryDto(
    Guid Id,
    Guid SupplierId,
    Guid BusinessId,
    string ItemBarcode,
    int DeliveryAmount,
    DateTimeOffset DateDelivered,
    DateTimeOffset ExpirationDate,
    decimal BatchWorth,
    string ConsignmentSchedule,
    DateTimeOffset? ReturnByDate,
    bool IsDeleted,
    DateTimeOffset LastUpdatedAt
);


public record UpdateDeliveryDto(
    string ItemBarcode,
    int DeliveryAmount,
    DateTimeOffset DateDelivered,
    DateTimeOffset ExpirationDate,
    decimal BatchWorth,
    string ConsignmentSchedule,
    DateTimeOffset? ReturnByDate
);
