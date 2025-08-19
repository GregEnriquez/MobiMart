using System;
using MobiMart.Api.Dtos;
using MobiMart.Api.Entities;

namespace MobiMart.Api.Mapping;

public static class WholeSaleInventoryMapping
{
    public static WholeSaleInventory ToEntity(this CreateWholeSaleInventoryDto newInv)
    {
        return new WholeSaleInventory()
        {
            SupplierId = newInv.SupplierId,
            ItemName = newInv.ItemName,
            DeliveryAmount = newInv.DeliveryAmount,
            DateDelivered = newInv.DateDelivered,
            ExpirationDate = newInv.ExpirationDate,
            BatchWorth = newInv.BatchWorth,
            ItemType = newInv.ItemType
        };
    }


    public static WholeSaleInventory ToEntity(this UpdateWholeSaleInventoryDto updatedInv, int id, int supplierId)
    {
        return new WholeSaleInventory()
        {
            Id = id,
            SupplierId = supplierId,
            ItemName = updatedInv.ItemName,
            DeliveryAmount = updatedInv.DeliveryAmount,
            DateDelivered = updatedInv.DateDelivered,
            ExpirationDate = updatedInv.ExpirationDate,
            BatchWorth = updatedInv.BatchWorth,
            ItemType = updatedInv.ItemType
        };
    }


    public static WholeSaleInventoryDto ToDto(this WholeSaleInventory inv)
    {
        return new WholeSaleInventoryDto(
            inv.Id,
            inv.SupplierId,
            inv.ItemName,
            inv.DeliveryAmount,
            inv.DateDelivered,
            inv.ExpirationDate,
            inv.BatchWorth,
            inv.ItemType
        );
    }
}
