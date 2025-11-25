using Microsoft.EntityFrameworkCore;
using MobiMart.Api.Data;
using MobiMart.Api.Dtos;
using MobiMart.Api.Entities;
using MobiMart.Api.Mapping;

namespace MobiMart.Api.Endpoints;

public static class InventoryEndpoints
{
    public static void MapInventoryEndpoints(this WebApplication app)
    {
        // --- ITEMS ---
        var items = app.MapGroup("items");

        items.MapGet("/{id:guid}", async (Guid id, MobiMartContext db) =>
        {
            return await db.Items.FindAsync(id) is Item i ? Results.Ok(i.ToDto()) : Results.NotFound();
        });

        items.MapPost("/", async (CreateItemDto dto, MobiMartContext db) =>
        {
            var entity = dto.ToEntity();
            entity.LastUpdatedAt = DateTimeOffset.UtcNow;
            db.Items.Add(entity);
            await db.SaveChangesAsync();
            return Results.Ok(entity.ToDto());
        });

        items.MapPut("/{id:guid}", async (Guid id, CreateItemDto dto, MobiMartContext db) =>
        {
            var existing = await db.Items.FindAsync(id);
            if (existing is null) return Results.NotFound();

            var updated = dto.ToEntity();
            updated.Id = id;
            updated.LastUpdatedAt = DateTimeOffset.UtcNow;

            db.Entry(existing).CurrentValues.SetValues(updated);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        items.MapDelete("/{id:guid}", async (Guid id, MobiMartContext db) =>
        {
            await db.Items.Where(x => x.Id == id)
                .ExecuteUpdateAsync(s => s.SetProperty(i => i.IsDeleted, true).SetProperty(i => i.LastUpdatedAt, DateTimeOffset.UtcNow));
            return Results.NoContent();
        });

        // --- INVENTORY ---
        var inventory = app.MapGroup("inventory");

        inventory.MapGet("/{id:guid}", async (Guid id, MobiMartContext db) =>
        {
            return await db.Inventories.FindAsync(id) is Inventory i ? Results.Ok(i.ToDto()) : Results.NotFound();
        });

        inventory.MapPost("/", async (CreateInventoryDto dto, MobiMartContext db) =>
        {
            var entity = dto.ToEntity();
            entity.LastUpdatedAt = DateTimeOffset.UtcNow;
            db.Inventories.Add(entity);
            await db.SaveChangesAsync();
            return Results.Ok(entity.ToDto());
        });

        inventory.MapPut("/{id:guid}", async (Guid id, CreateInventoryDto dto, MobiMartContext db) =>
        {
            var existing = await db.Inventories.FindAsync(id);
            if (existing is null) return Results.NotFound();

            var updated = dto.ToEntity();
            updated.Id = id;
            updated.LastUpdatedAt = DateTimeOffset.UtcNow;

            db.Entry(existing).CurrentValues.SetValues(updated);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        inventory.MapDelete("/{id:guid}", async (Guid id, MobiMartContext db) =>
        {
            await db.Inventories.Where(x => x.Id == id)
                .ExecuteUpdateAsync(s => s.SetProperty(i => i.IsDeleted, true).SetProperty(i => i.LastUpdatedAt, DateTimeOffset.UtcNow));
            return Results.NoContent();
        });

        items.RequireAuthorization();
        inventory.RequireAuthorization();
    }
}