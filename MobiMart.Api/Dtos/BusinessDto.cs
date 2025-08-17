using System.ComponentModel.DataAnnotations;

namespace MobiMart.Api.Dtos;

public record class BusinessDto(
    int Id,
    string Name,
    string Address,
    string Code
);

public record class UpdateBusinessDto(
    [Required] string Name,
    [Required] string Address,
    [Required] string Code
);

public record class CreateBusinessDto(
    [Required] string Name,
    [Required] string Address,
    [Required] string Code // the backend should be able to generate the code? nah I think the modelView should be the one to do that
);