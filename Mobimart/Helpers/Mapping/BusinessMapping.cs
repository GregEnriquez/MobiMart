using System;
using MobiMart.Helpers.Dtos;
using MobiMart.Model;

namespace MobiMart.Helpers.Mapping;

public static class BusinessMapping
{
    public static CreateBusinessDto ToDto(this Business business)
    {
        return new CreateBusinessDto(
            business.Id,
            business.Name,
            business.Address,
            business.Code,
            business.IsDeleted,
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


    public static Business ToEntity(this BusinessDto newBusiness)
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
