using System;
using MobiMart.Api.Dtos;
using MobiMart.Api.Entities;

namespace MobiMart.Api.Mapping;

public static class DeliveryMapping
{

    public static DeliveryDto ToDto(this Delivery entity)
    {
        return new DeliveryDto(
            entity.Id,
            entity.SupplierId,
            entity.BusinessId,
            entity.ItemBarcode,
            entity.DeliveryAmount,
            entity.DateDelivered,
            entity.ExpirationDate,
            entity.BatchWorth,
            entity.ConsignmentSchedule,
            entity.ReturnByDate,
            entity.LastUpdatedAt,
            entity.IsDeleted
        );
    }

    public static Delivery ToEntity(this CreateDeliveryDto dto)
    {
        return new Delivery
        {
            Id = dto.Id,
            SupplierId = dto.SupplierId,
            BusinessId = dto.BusinessId,
            ItemBarcode = dto.ItemBarcode,
            DeliveryAmount = dto.DeliveryAmount,
            DateDelivered = dto.DateDelivered,
            ExpirationDate = dto.ExpirationDate,
            BatchWorth = dto.BatchWorth,
            ConsignmentSchedule = dto.ConsignmentSchedule,
            ReturnByDate = dto.ReturnByDate,
            LastUpdatedAt = dto.LastUpdatedAt,
            IsDeleted = dto.IsDeleted
        };
    }    

}
