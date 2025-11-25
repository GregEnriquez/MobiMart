namespace MobiMart.Api.Dtos;

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
    bool IsDeleted
);


public record class UpdateDescriptionDto(
    string Text  
);
