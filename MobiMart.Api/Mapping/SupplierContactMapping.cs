using System;
using MobiMart.Api.Dtos;
using MobiMart.Api.Entities;

namespace MobiMart.Api.Mapping;

public static class SupplierContactMapping
{
    public static SupplierContact ToEntity(this CreateSupplierContactDto newContact)
    {
        return new SupplierContact()
        {
            SupplierId = newContact.SupplierId,
            Name = newContact.Name,
            Email = newContact.Email
        };
    }


    public static SupplierContact ToEntity(this UpdateSupplierContactDto updatedContact, int id, int supplierId)
    {
        return new SupplierContact()
        {
            Id = id,
            SupplierId = supplierId,
            Name = updatedContact.Name,
            Email = updatedContact.Email
        };
    }


    public static SupplierContactDto ToDto(this SupplierContact contact)
    {
        return new SupplierContactDto(
            contact.Id,
            contact.SupplierId,
            contact.Name,
            contact.Email
        );
    }
}
