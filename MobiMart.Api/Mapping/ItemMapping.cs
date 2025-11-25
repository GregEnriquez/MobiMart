using System;
using MobiMart.Api.Dtos;
using MobiMart.Api.Entities;

namespace MobiMart.Api.Mapping;

public static class ItemMapping
{
    public static ItemDto ToDto(this Item entity)
    {
        return new ItemDto(
            entity.Id,
            entity.BusinessId,
            entity.Barcode,
            entity.Name,
            entity.Type,
            entity.RetailPrice,
            entity.DescriptionId,
            entity.LastUpdatedAt,
            entity.IsDeleted
        );
    }

    public static Item ToEntity(this CreateItemDto dto)
    {
        return new Item
        {
            Id = dto.Id,
            BusinessId = dto.BusinessId,
            Barcode = dto.Barcode,
            Name = dto.Name,
            Type = dto.Type,
            RetailPrice = dto.RetailPrice,
            DescriptionId = dto.DescriptionId,
            LastUpdatedAt = dto.LastUpdatedAt,
            IsDeleted = dto.IsDeleted
        };
    }

}
