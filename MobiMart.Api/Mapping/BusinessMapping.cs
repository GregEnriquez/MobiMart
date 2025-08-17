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
            business.Name,
            business.Address,
            business.Code
        );
    }


    public static Business ToEntity(this CreateBusinessDto newBusiness)
    {
        return new Business()
        {
            Name = newBusiness.Name,
            Address = newBusiness.Address,
            Code = newBusiness.Code
        };
    }


    public static Business ToEntity(this UpdateBusinessDto newBusiness, int id)
    {
        return new Business()
        {
            Id = id,
            Name = newBusiness.Name,
            Address = newBusiness.Address,
            Code = newBusiness.Code
        };
    }
}
