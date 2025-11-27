using System;
using MobiMart.Helpers.Dtos;
using MobiMart.Model;

namespace MobiMart.Helpers.Mapping;

public static class DeliveryMapping
{

    public static CreateDeliveryDto ToDto(this Delivery entity)
    {
        return new CreateDeliveryDto(
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
            entity.IsDeleted,
            entity.LastUpdatedAt
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



    public static Delivery ToEntity(this DeliveryDto dto)
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
