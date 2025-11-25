using System;
using MobiMart.Api.Dtos;
using MobiMart.Api.Entities;

namespace MobiMart.Api.Mapping;

public static class BusinessMapping
{
    public static BusinessDto ToDto(this Business business)
    {
        return new BusinessDto(
            business.Id,
            business.IsDeleted,
            business.Name,
            business.Address,
            business.Code,
            business.LastUpdatedAt
        );
    }


    public static Business ToEntity(this CreateBusinessDto newBusiness)
    {
        return new Business()
        {
            Id = newBusiness.Id,
            LastUpdatedAt = newBusiness.LastUpdatedAt,
            IsDeleted = newBusiness.IsDeleted,
            Name = newBusiness.Name,
            Address = newBusiness.Address,
            Code = newBusiness.Code
        };
    }


    public static Business ToEntity(this UpdateBusinessDto newBusiness, Guid id)
    {
        return new Business()
        {
            Id = id,
            LastUpdatedAt = DateTimeOffset.UtcNow,
            Name = newBusiness.Name,
            Address = newBusiness.Address,
            Code = newBusiness.Code,
            IsDeleted = newBusiness.IsDeleted
        };
    }
}
