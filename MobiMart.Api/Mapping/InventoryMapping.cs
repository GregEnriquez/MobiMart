using System;
using MobiMart.Api.Dtos;
using MobiMart.Api.Entities;

namespace MobiMart.Api.Mapping;

public static class InventoryMapping
{
    public static InventoryDto ToDto(this Inventory inv)
    {
        return new InventoryDto(
            inv.Id,
            inv.BusinessId,
            inv.DeliveryId,
            inv.ItemBarcode,
            inv.TotalAmount,
            inv.LastUpdatedAt,
            inv.IsDeleted
        );
    }


    public static Inventory ToEntity(this CreateInventoryDto newInv)
    {
        return new Inventory()
        {
            Id = newInv.Id,
            LastUpdatedAt = newInv.LastUpdatedAt,
            IsDeleted = newInv.IsDeleted,
            BusinessId = newInv.BusinessId,
            DeliveryId = newInv.DeliveryId,
            ItemBarcode = newInv.ItemBarcode,
            TotalAmount = newInv.TotalAmount
        };
    }


    public static Inventory ToEntity(this UpdateInventoryDto newInv, Guid id, Guid businessId, Guid deliveryId, string itemBarcode)
    {
        return new Inventory()
        {
            Id = id,
            LastUpdatedAt = DateTimeOffset.UtcNow,
            IsDeleted = false,
            BusinessId = businessId,
            DeliveryId = deliveryId,
            ItemBarcode = itemBarcode,
            TotalAmount = newInv.TotalAmount
        };
    }
}
