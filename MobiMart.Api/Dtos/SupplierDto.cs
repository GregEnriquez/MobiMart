
using System.ComponentModel.DataAnnotations;
using MobiMart.Api.Entities;

namespace MobiMart.Api.Dtos;

public record SupplierDto(
    Guid Id,
    Guid BusinessId,
    string Type,
    string Name,
    string Email,
    string Socials,
    string Number,
    DateTimeOffset LastUpdatedAt,
    bool IsDeleted
);

public record CreateSupplierDto(
    Guid Id,
    Guid BusinessId,
    string Type,
    string Name,
    string Email,
    string Socials,
    string Number,
    bool IsDeleted
);


public record class UpdateSupplierDto(
    string Type,
    string Name,
    string Email,
    string Socials,
    string Number
);
