using System;
using MobiMart.Helpers.Dtos;
using MobiMart.Model;

namespace MobiMart.Helpers.Mapping;

public static class ItemMapping
{
    public static CreateItemDto ToDto(this Item entity)
    {
        return new CreateItemDto(
            entity.Id,
            entity.BusinessId,
            entity.Barcode,
            entity.Name,
            entity.Type,
            entity.RetailPrice,
            entity.DescriptionId,
            entity.IsDeleted,
            entity.LastUpdatedAt
        );
    }

    public static Item ToEntity(this CreateItemDto dto)
    {
        return new Item
        {
            Id = dto.Id,
            LastUpdatedAt = dto.LastUpdatedAt,
            IsDeleted = dto.IsDeleted,
            BusinessId = dto.BusinessId,
            Barcode = dto.Barcode,
            Name = dto.Name,
            Type = dto.Type,
            RetailPrice = dto.RetailPrice,
            DescriptionId = dto.DescriptionId
        };
    }



    public static Item ToEntity(this ItemDto dto)
    {
        return new Item
        {
            Id = dto.Id,
            LastUpdatedAt = dto.LastUpdatedAt,
            IsDeleted = dto.IsDeleted,
            BusinessId = dto.BusinessId,
            Barcode = dto.Barcode,
            Name = dto.Name,
            Type = dto.Type,
            RetailPrice = dto.RetailPrice,
            DescriptionId = dto.DescriptionId
        };
    }

}
