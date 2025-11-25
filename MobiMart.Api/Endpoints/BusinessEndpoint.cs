using Microsoft.EntityFrameworkCore;
using MobiMart.Api.Data;
using MobiMart.Api.Dtos;
using MobiMart.Api.Entities;
using MobiMart.Api.Mapping;

namespace MobiMart.Api.Endpoints;

public static class BusinessEndpoints
{
    public static RouteGroupBuilder MapBusinessEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("businesses");

        group.MapGet("/{id:guid}", async (Guid id, MobiMartContext db) =>
        {
            return await db.Businesses.FindAsync(id) is Business b 
                ? Results.Ok(b.ToDto()) 
                : Results.NotFound();
        });

        group.MapPost("/", async (CreateBusinessDto dto, MobiMartContext db) =>
        {
            var entity = dto.ToEntity();
            entity.LastUpdatedAt = DateTimeOffset.UtcNow;
            db.Businesses.Add(entity);
            await db.SaveChangesAsync();
            return Results.Ok(entity.ToDto());
        });

        group.MapPut("/{id:guid}", async (Guid id, CreateBusinessDto dto, MobiMartContext db) =>
        {
            var existing = await db.Businesses.FindAsync(id);
            if (existing is null) return Results.NotFound();

            // Reuse CreateDto logic for update
            var updated = dto.ToEntity(); 
            updated.Id = id; // Ensure ID matches URL
            updated.LastUpdatedAt = DateTimeOffset.UtcNow;

            db.Entry(existing).CurrentValues.SetValues(updated);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        group.MapDelete("/{id:guid}", async (Guid id, MobiMartContext db) =>
        {
            await db.Businesses.Where(x => x.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(b => b.IsDeleted, true)
                    .SetProperty(b => b.LastUpdatedAt, DateTimeOffset.UtcNow));
            return Results.NoContent();
        });

        return group;
    }
}