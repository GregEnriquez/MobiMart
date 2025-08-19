using System;
using Microsoft.EntityFrameworkCore;
using MobiMart.Api.Data;
using MobiMart.Api.Dtos;
using MobiMart.Api.Entities;
using MobiMart.Api.Mapping;

namespace MobiMart.Api.Endpoints;

public static class WholeSaleInventoriesEndpoints
{

    const string GetWholeSaleInventoriesEndpointName = "GetWholeSaleInventory";

    public static RouteGroupBuilder MapWholeSaleInventoriesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("wholesale-inventories");

        // GET /wholesale-inventories
        group.MapGet("/", () => {} );


        // GET /wholesale-inventories/1
        group.MapGet("/{id}", async (int id, MobiMartContext dbContext) =>
        {
            WholeSaleInventory? inv = await dbContext.WholeSaleInventories.FindAsync(id);

            return inv is null ? Results.NotFound() : Results.Ok(inv.ToDto());
        })
        .WithName(GetWholeSaleInventoriesEndpointName);


        // POST /wholesale-inventories
        group.MapPost("/", async (CreateWholeSaleInventoryDto newInv, MobiMartContext dbContext) =>
        {
            WholeSaleInventory inv = newInv.ToEntity();

            dbContext.WholeSaleInventories.Add(inv);
            await dbContext.SaveChangesAsync();

            return Results.CreatedAtRoute(GetWholeSaleInventoriesEndpointName, new { id = inv.Id }, inv.ToDto());
        });


        // PUT /wholesale-inventories/1
        group.MapPut("/{id}", async (int id, UpdateWholeSaleInventoryDto updatedInv, MobiMartContext dbContext) =>
        {
            var existingInv = await dbContext.WholeSaleInventories.FindAsync(id);
            if (existingInv is null) return Results.NotFound();

            dbContext.Entry(existingInv)
                     .CurrentValues
                     .SetValues(updatedInv.ToEntity(id, existingInv.SupplierId));
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });


        // DELETE /wholesale-inventories/1
        group.MapDelete("/{id}", async (int id, MobiMartContext dbContext) =>
        {
            await dbContext.WholeSaleInventories
                           .Where(inv => inv.Id == id)
                           .ExecuteDeleteAsync();

            return Results.NoContent();
        });


        return group;
    }
}
