namespace MobiMart.Api.Dtos;

public record class DescriptionDto(
    int Id,
    int ItemId,
    string Text
);


public record class CreateDescriptionDto(
    int ItemId,
    string Text
);


public record class UpdateDescriptionDto(
    string Text  
);
