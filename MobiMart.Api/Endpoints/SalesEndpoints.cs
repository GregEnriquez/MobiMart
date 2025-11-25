using Microsoft.EntityFrameworkCore;
using MobiMart.Api.Data;
using MobiMart.Api.Dtos;
using MobiMart.Api.Entities;
using MobiMart.Api.Mapping;

namespace MobiMart.Api.Endpoints;

public static class SalesEndpoints
{
    public static RouteGroupBuilder MapSalesEndpoints(this WebApplication app)
    {
        var sales = app.MapGroup("transactions");

        sales.MapGet("/{id:guid}", async (Guid id, MobiMartContext db) =>
        {
            return await db.SalesTransactions.FindAsync(id) is SalesTransaction s ? Results.Ok(s.ToDto()) : Results.NotFound();
        });

        sales.MapPost("/", async (CreateSalesTransactionDto dto, MobiMartContext db) =>
        {
            var entity = dto.ToEntity();
            entity.LastUpdatedAt = DateTimeOffset.UtcNow;
            db.SalesTransactions.Add(entity);
            await db.SaveChangesAsync();
            return Results.Ok(entity.ToDto());
        });

        // Note: Usually we don't edit sales history, but for sync we might receive updates
        sales.MapPut("/{id:guid}", async (Guid id, CreateSalesTransactionDto dto, MobiMartContext db) =>
        {
            var existing = await db.SalesTransactions.FindAsync(id);
            if (existing is null) return Results.NotFound();

            var updated = dto.ToEntity();
            updated.Id = id;
            updated.LastUpdatedAt = DateTimeOffset.UtcNow;
            db.Entry(existing).CurrentValues.SetValues(updated);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        sales.MapDelete("/{id:guid}", async (Guid id, MobiMartContext db) =>
        {
            await db.SalesTransactions.Where(x => x.Id == id)
                .ExecuteUpdateAsync(s => s.SetProperty(i => i.IsDeleted, true).SetProperty(i => i.LastUpdatedAt, DateTimeOffset.UtcNow));
            return Results.NoContent();
        });


        return sales;
    }
}