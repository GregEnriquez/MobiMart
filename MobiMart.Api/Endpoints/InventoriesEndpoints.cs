using System;
using Microsoft.EntityFrameworkCore;
using MobiMart.Api.Data;
using MobiMart.Api.Dtos;
using MobiMart.Api.Entities;
using MobiMart.Api.Mapping;

namespace MobiMart.Api.Endpoints;

public static class InventoriesEndpoints
{
    const string GetInventoriesEndpointName = "GetInventory";

    public static RouteGroupBuilder MapInventoriesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("inventories");

        // GET /inventories
        group.MapGet("/", () => {} );


        // GET /inventories/1
        group.MapGet("/{id}", async (int id, MobiMartContext dbContext) =>
        {
            Inventory? inv = await dbContext.Inventories.FindAsync(id);

            return inv is null ? Results.NotFound() : Results.Ok(inv.ToDto());
        })
        .WithName(GetInventoriesEndpointName);


        // POST /inventories
        group.MapPost("/", async (CreateInventoryDto newInv, MobiMartContext dbContext) =>
        {
            Inventory inv = newInv.ToEntity();

            dbContext.Inventories.Add(inv);
            await dbContext.SaveChangesAsync();

            return Results.CreatedAtRoute(GetInventoriesEndpointName, new { id = inv.Id }, inv.ToDto());
        });


        // PUT /inventories/1
        group.MapPut("/{id}", async (int id, UpdateInventoryDto updatedInv, MobiMartContext dbContext) =>
        {
            var existingInv = await dbContext.Inventories.FindAsync(id);
            if (existingInv is null) return Results.NotFound();

            dbContext.Entry(existingInv)
                     .CurrentValues
                     .SetValues(updatedInv.ToEntity(id, existingInv.BusinessId, existingInv.DeliveryId, existingInv.DescriptionId));
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });


        // DELETE /inventories/1
        group.MapDelete("/{id}", async (int id, MobiMartContext dbContext) =>
        {
            await dbContext.Inventories
                           .Where(inv => inv.Id == id)
                           .ExecuteDeleteAsync();

            return Results.NoContent();
        });


        return group;
    }
}
