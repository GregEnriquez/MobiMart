
using System.ComponentModel.DataAnnotations;
using MobiMart.Api.Entities;

namespace MobiMart.Api.Dtos;

public record class SupplierDto(
    int Id,
    int BusinessId,
    string Type,
    string Name,
    string Email,
    string SocialsLink,
    string SocialsDescription
);


public record class CreateSupplierDto(
    [Required]int BusinessId,
    [Required]string Type,
    [Required]string Name,
    [Required]string Email,
    string SocialsLink,
    string SocialsDescription
);


public record class UpdateSupplierDto(
    string Type,
    string Name,
    string Email,
    string SocialsLink,
    string SocialsDescription
);
