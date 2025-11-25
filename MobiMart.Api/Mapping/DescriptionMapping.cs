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
            description.Text,
            description.LastUpdatedAt,
            description.IsDeleted
        );
    }


    public static Description ToEntity(this CreateDescriptionDto newDescription)
    {
        return new Description()
        {
            Id = newDescription.Id,
            LastUpdatedAt = DateTimeOffset.UtcNow,
            IsDeleted = newDescription.IsDeleted,
            ItemId = newDescription.ItemId,
            Text = newDescription.Text
        };
    }


    public static Description ToEntity(this UpdateDescriptionDto updatedDescription, Guid id, Guid itemId)
    {
        return new Description()
        {
            Id = id,
            LastUpdatedAt = DateTimeOffset.UtcNow,
            ItemId = itemId,
            Text = updatedDescription.Text
        };
    }
}
