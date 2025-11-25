using Microsoft.EntityFrameworkCore;
using MobiMart.Api.Data;
using MobiMart.Api.Dtos;
using MobiMart.Api.Entities;
using MobiMart.Api.Mapping;

namespace MobiMart.Api.Endpoints;

public static class SupplyChainEndpoints
{
    public static void MapSupplyChainEndpoints(this WebApplication app)
    {
        // --- SUPPLIERS ---
        var suppliers = app.MapGroup("suppliers");

        suppliers.MapGet("/{id:guid}", async (Guid id, MobiMartContext db) =>
        {
            return await db.Suppliers.FindAsync(id) is Supplier s ? Results.Ok(s.ToDto()) : Results.NotFound();
        });

        suppliers.MapPost("/", async (CreateSupplierDto dto, MobiMartContext db) =>
        {
            var entity = dto.ToEntity();
            entity.LastUpdatedAt = DateTimeOffset.UtcNow;
            db.Suppliers.Add(entity);
            await db.SaveChangesAsync();
            return Results.Ok(entity.ToDto());
        });

        suppliers.MapPut("/{id:guid}", async (Guid id, CreateSupplierDto dto, MobiMartContext db) =>
        {
            var existing = await db.Suppliers.FindAsync(id);
            if (existing is null) return Results.NotFound();

            var updated = dto.ToEntity();
            updated.Id = id;
            updated.LastUpdatedAt = DateTimeOffset.UtcNow;
            db.Entry(existing).CurrentValues.SetValues(updated);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        suppliers.MapDelete("/{id:guid}", async (Guid id, MobiMartContext db) =>
        {
            await db.Suppliers.Where(x => x.Id == id)
                .ExecuteUpdateAsync(s => s.SetProperty(i => i.IsDeleted, true).SetProperty(i => i.LastUpdatedAt, DateTimeOffset.UtcNow));
            return Results.NoContent();
        });

        // --- DELIVERIES ---
        var deliveries = app.MapGroup("deliveries");

        deliveries.MapGet("/{id:guid}", async (Guid id, MobiMartContext db) =>
        {
            return await db.Deliveries.FindAsync(id) is Delivery d ? Results.Ok(d.ToDto()) : Results.NotFound();
        });

        deliveries.MapPost("/", async (CreateDeliveryDto dto, MobiMartContext db) =>
        {
            var entity = dto.ToEntity();
            entity.LastUpdatedAt = DateTimeOffset.UtcNow;
            db.Deliveries.Add(entity);
            await db.SaveChangesAsync();
            return Results.Ok(entity.ToDto());
        });

        deliveries.MapPut("/{id:guid}", async (Guid id, CreateDeliveryDto dto, MobiMartContext db) =>
        {
            var existing = await db.Deliveries.FindAsync(id);
            if (existing is null) return Results.NotFound();

            var updated = dto.ToEntity();
            updated.Id = id;
            updated.LastUpdatedAt = DateTimeOffset.UtcNow;
            db.Entry(existing).CurrentValues.SetValues(updated);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        deliveries.MapDelete("/{id:guid}", async (Guid id, MobiMartContext db) =>
        {
            await db.Deliveries.Where(x => x.Id == id)
                .ExecuteUpdateAsync(s => s.SetProperty(i => i.IsDeleted, true).SetProperty(i => i.LastUpdatedAt, DateTimeOffset.UtcNow));
            return Results.NoContent();
        });

        suppliers.RequireAuthorization();
        deliveries.RequireAuthorization();
    }
}