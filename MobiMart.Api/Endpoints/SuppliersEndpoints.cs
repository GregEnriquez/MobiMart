using System;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using MobiMart.Api.Data;
using MobiMart.Api.Dtos;
using MobiMart.Api.Entities;
using MobiMart.Api.Mapping;

namespace MobiMart.Api.Endpoints;

public static class SuppliersEndpoints
{
    const string GetSupplierEndpointName = "GetSupplier";

    public static RouteGroupBuilder MapSuppliersEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("suppliers");

        // GET /suppliers
        group.MapGet("/", () => {} );


        // GET /suppliers/1
        group.MapGet("/{id}", async (int id, MobiMartContext dbContext) =>
        {
            Supplier? supplier = await dbContext.Suppliers.FindAsync(id);
            Socials? socials = await dbContext.SocialsSet.FindAsync(supplier!.SocialsId);

            return supplier is null ? Results.NotFound() : Results.Ok(supplier.ToDto(socials!));
        })
        .WithName(GetSupplierEndpointName);


        // POST /suppliers
        group.MapPost("/", async (CreateSupplierDto newSupplier, MobiMartContext dbContext) =>
        {
            Supplier supplier = newSupplier.ToEntity();

            Socials socials = new()
            {
                Link = newSupplier.SocialsLink,
                Description = newSupplier.SocialsDescription
            };
            supplier.Socials = socials;
            supplier.SocialsId = socials.Id;

            dbContext.SocialsSet.Add(socials);
            await dbContext.SaveChangesAsync();

            dbContext.Suppliers.Add(supplier);
            await dbContext.SaveChangesAsync();

            return Results.CreatedAtRoute(GetSupplierEndpointName, new { id = supplier.Id }, supplier.ToDto(socials));
        });


        // PUT /suppliers/1
        group.MapPut("/{id}", async (int id, UpdateSupplierDto updatedSupplier, MobiMartContext dbContext) =>
        {
            var existingSupplier = await dbContext.Suppliers.FindAsync(id);
            if (existingSupplier is null) return Results.NotFound();

            dbContext.Entry(existingSupplier)
                     .CurrentValues
                     .SetValues(updatedSupplier.ToEntity(id, existingSupplier.BusinessId, existingSupplier.Socials!));
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });


        // DELETE /suppliers/1
        group.MapDelete("/{id}", async (int id, MobiMartContext dbContext) =>
        {
            await dbContext.SocialsSet
                           .Where(socials => socials.Id == dbContext.Suppliers.Find(id)!.SocialsId)
                           .ExecuteDeleteAsync();

            await dbContext.Suppliers
                           .Where(supplier => supplier.Id == id)
                           .ExecuteDeleteAsync();

            return Results.NoContent();
        });

        return group;
    }
}
