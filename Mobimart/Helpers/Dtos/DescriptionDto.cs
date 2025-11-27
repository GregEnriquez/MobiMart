namespace MobiMart.Helpers.Dtos;

public record DescriptionDto(
    Guid Id,
    Guid ItemId,
    string Text,
    DateTimeOffset LastUpdatedAt,
    bool IsDeleted
);

public record CreateDescriptionDto(
    Guid Id,
    Guid ItemId,
    string Text,
    bool IsDeleted,
    DateTimeOffset LastUpdatedAt
);


public record class UpdateDescriptionDto(
    string Text  
);
