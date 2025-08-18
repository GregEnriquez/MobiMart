using System;
using MobiMart.Api.Dtos;
using MobiMart.Api.Entities;

namespace MobiMart.Api.Mapping;

public static class DescriptionMapping
{
    public static DescriptionDto ToDto(this Description description)
    {
        return new DescriptionDto(
            description.Id,
            description.ItemId,
            description.Text
        );
    }


    public static Description ToEntity(this CreateDescriptionDto newDescription)
    {
        return new Description()
        {
            ItemId = newDescription.ItemId,
            Text = newDescription.Text
        };
    }


    public static Description ToEntity(this UpdateDescriptionDto updatedDescription, int id, int itemId)
    {
        return new Description()
        {
            Id = id,
            ItemId = itemId,
            Text = updatedDescription.Text
        };
    }
}
