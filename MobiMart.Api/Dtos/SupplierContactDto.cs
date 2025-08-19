namespace MobiMart.Api.Dtos;

public record class SupplierContactDto(
    int Id,
    int SupplierId,
    string Name,
    string Email
);


public record class CreateSupplierContactDto(
    int SupplierId,
    string Name,
    string Email
);


public record class UpdateSupplierContactDto(
    string Name,
    string Email
);
