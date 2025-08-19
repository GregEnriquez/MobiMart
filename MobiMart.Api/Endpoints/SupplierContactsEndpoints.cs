using System;
using Microsoft.EntityFrameworkCore;
using MobiMart.Api.Data;
using MobiMart.Api.Dtos;
using MobiMart.Api.Entities;
using MobiMart.Api.Mapping;

namespace MobiMart.Api.Endpoints;

public static class SupplierContactsEndpoints
{
    const string GetSupplierContactEndpointName = "GetSupplierContact";

    public static RouteGroupBuilder MapSupplierContactsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("suppliers/contacts");

        // GET /suppliers
        group.MapGet("/", () => {} );


        // GET /suppliers/contacts/1
        group.MapGet("/{id}", async (int id, MobiMartContext dbContext) =>
        {
            SupplierContact? contact = await dbContext.SupplierContacts.FindAsync(id);

            return contact is null ? Results.NotFound() : Results.Ok(contact.ToDto());
        })
        .WithName(GetSupplierContactEndpointName);


        // POST /suppliers/contacts
        group.MapPost("/", async (CreateSupplierContactDto newContact, MobiMartContext dbContext) =>
        {
            SupplierContact contact = newContact.ToEntity();

            dbContext.SupplierContacts.Add(contact);
            await dbContext.SaveChangesAsync();

            return Results.CreatedAtRoute(GetSupplierContactEndpointName, new { id = contact.Id }, contact.ToDto());
        });


        // PUT /suppliers/contacts/1
        group.MapPut("/{id}", async (int id, UpdateSupplierContactDto updatedContact, MobiMartContext dbContext) =>
        {
            var existingContact = await dbContext.SupplierContacts.FindAsync(id);
            if (existingContact is null) return Results.NotFound();

            dbContext.Entry(existingContact)
                     .CurrentValues
                     .SetValues(updatedContact.ToEntity(id, existingContact.SupplierId));
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });


        // DELETE /suppliers/contacts/1
        group.MapDelete("/{id}", async (int id, MobiMartContext dbContext) =>
        {
            await dbContext.SupplierContacts
                           .Where(contact => contact.Id == id)
                           .ExecuteDeleteAsync();

            return Results.NoContent();
        });

        return group;
    }
}
