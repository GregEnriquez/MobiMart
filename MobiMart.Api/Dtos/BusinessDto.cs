using System.ComponentModel.DataAnnotations;

namespace MobiMart.Api.Dtos;

public record BusinessDto(
    Guid Id,
    bool IsDeleted,
    string Name,
    string Address,
    string Code,
    DateTimeOffset LastUpdatedAt
);

public record CreateBusinessDto(
    Guid Id, // Client provides this!
    string Name,
    string Address,
    string Code,
    bool IsDeleted
);

public record UpdateBusinessDto(
    string Name,
    string Address,
    string Code,
    bool IsDeleted
);