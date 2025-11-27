using System;
using MobiMart.Helpers.Dtos;
using MobiMart.Model;

namespace MobiMart.Helpers.Mapping;

public static class DescriptionMapping
{
    public static CreateDescriptionDto ToDto(this Description description)
    {
        return new CreateDescriptionDto(
            description.Id,
            description.ItemId,
            description.Text,
            description.IsDeleted,
            description.LastUpdatedAt
        );
    }


    public static Description ToEntity(this CreateDescriptionDto newDescription)
    {
        return new Description()
        {
            Id = newDescription.Id,
            LastUpdatedAt = newDescription.LastUpdatedAt,
            IsDeleted = newDescription.IsDeleted,
            ItemId = newDescription.ItemId,
            Text = newDescription.Text
        };
    }



    public static Description ToEntity(this DescriptionDto newDescription)
    {
        return new Description()
        {
            Id = newDescription.Id,
            LastUpdatedAt = newDescription.LastUpdatedAt,
            IsDeleted = newDescription.IsDeleted,
            ItemId = newDescription.ItemId,
            Text = newDescription.Text
        };
    }
}
