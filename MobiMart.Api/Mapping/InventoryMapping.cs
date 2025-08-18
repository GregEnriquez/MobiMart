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
            inv.DescriptionId,
            inv.TotalAmount,
            inv.ItemName,
            inv.RetailPrice,
            inv.ItemType
        );
    }


    public static Inventory ToEntity(this CreateInventoryDto newInv)
    {
        return new Inventory()
        {
            BusinessId = newInv.BusinessId,
            DeliveryId = newInv.DeliveryId,
            DescriptionId = newInv.DescriptionId,
            TotalAmount = newInv.TotalAmount,
            ItemName = newInv.ItemName,
            RetailPrice = newInv.RetailPrice,
            ItemType = newInv.ItemType
        };
    }


    public static Inventory ToEntity(this UpdateInventoryDto newInv, int id, int businessId, int deliveryId, int descriptionId)
    {
        return new Inventory()
        {
            Id = id,
            BusinessId = businessId,
            DeliveryId = deliveryId,
            DescriptionId = descriptionId,
            TotalAmount = newInv.TotalAmount,
            ItemName = newInv.ItemName,
            RetailPrice = newInv.RetailPrice,
            ItemType = newInv.ItemType
        };
    }
}
