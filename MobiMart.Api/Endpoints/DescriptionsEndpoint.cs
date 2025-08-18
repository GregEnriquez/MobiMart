using System;
using Microsoft.EntityFrameworkCore;
using MobiMart.Api.Data;
using MobiMart.Api.Dtos;
using MobiMart.Api.Entities;
using MobiMart.Api.Mapping;

namespace MobiMart.Api.Endpoints;

public static class DescriptionsEndpoint
{
    const string GetDescriptionsEndpointName = "GetDescription";

    public static RouteGroupBuilder MapDescriptionsEndpoint(this WebApplication app)
    {
        var group = app.MapGroup("descriptions");

        // GET /descriptions
        group.MapGet("/", () => { });


        // GET /descriptions/1
        group.MapGet("/{id}", async (int id, MobiMartContext dbContext) =>
        {
            Description? desc = await dbContext.Descriptions.FindAsync(id);

            return desc is null ? Results.NotFound() : Results.Ok(desc.ToDto());
        })
        .WithName(GetDescriptionsEndpointName);


        // POST /descriptions
        group.MapPost("/", async (CreateDescriptionDto newDesc, MobiMartContext dbContext) =>
        {
            Description desc = newDesc.ToEntity();

            dbContext.Descriptions.Add(desc);
            await dbContext.SaveChangesAsync();

            return Results.CreatedAtRoute(GetDescriptionsEndpointName, new { id = desc.Id }, desc.ToDto());
        });


        // PUT /descriptions
        group.MapPut("/{id}", async (int id, UpdateDescriptionDto updatedDesc, MobiMartContext dbContext) =>
        {
            var existingDesc = await dbContext.Descriptions.FindAsync(id);
            if (existingDesc is null) return Results.NotFound();

            dbContext.Entry(existingDesc)
                     .CurrentValues
                     .SetValues(updatedDesc.ToEntity(id, existingDesc.ItemId));
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });


        // DELETE /descriptions
        group.MapDelete("/{id}", async (int id, MobiMartContext dbContext) =>
        {
            await dbContext.Descriptions
                           .Where(desc => desc.Id == id)
                           .ExecuteDeleteAsync();
            return Results.NoContent();
        });


        return group;
    }
}
