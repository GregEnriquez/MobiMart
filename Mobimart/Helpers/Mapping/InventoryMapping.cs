using System;
using MobiMart.Helpers.Dtos;
using MobiMart.Model;

namespace MobiMart.Helpers.Mapping;

public static class InventoryMapping
{
    public static CreateInventoryDto ToDto(this Inventory inv)
    {
        return new CreateInventoryDto(
            inv.Id,
            inv.BusinessId,
            inv.DeliveryId,
            inv.ItemBarcode,
            inv.TotalAmount,
            inv.IsDeleted,
            inv.LastUpdatedAt
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



    public static Inventory ToEntity(this InventoryDto newInv)
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
}
