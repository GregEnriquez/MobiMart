using System;
using Microsoft.EntityFrameworkCore;
using MobiMart.Api.Data;
using MobiMart.Api.Dtos;
using MobiMart.Api.Entities;
using MobiMart.Api.Mapping;

namespace MobiMart.Api.Endpoints;

public static class BusinessesEndpoints
{

    const string GetBusinessesEndpointName = "GetBusiness";
    public static RouteGroupBuilder MapBusinessesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("businesses");
        // GET /businesses
        group.MapGet("/", () => { });


        // GET /businesses/1
        group.MapGet("/{id}", async (int id, MobiMartContext dbContext) =>
        {
            Business? business = await dbContext.Businesses.FindAsync(id);

            return business is null ? Results.NotFound() : Results.Ok(business);
        })
        .WithName(GetBusinessesEndpointName);


        // POST /businesses
        group.MapPost("/", async (CreateBusinessDto newBusiness, MobiMartContext dbContext) =>
        {
            Business business = newBusiness.ToEntity();

            dbContext.Businesses.Add(business);
            await dbContext.SaveChangesAsync();

            return Results.CreatedAtRoute(GetBusinessesEndpointName, new { id = business.Id }, business.ToDto());
        });


        // PUT /businesses/1
        group.MapPut("/{id}", async (int id, UpdateBusinessDto updatedBusiness, MobiMartContext dbContext) =>
        {
            var existingBusiness = await dbContext.Businesses.FindAsync(id);
            if (existingBusiness is null) return Results.NotFound();

            dbContext.Entry(existingBusiness)
                     .CurrentValues
                     .SetValues(updatedBusiness.ToEntity(id));
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });


        // DELETE /businesses/1
        group.MapDelete("/{id}", async (int id, MobiMartContext dbContext) =>
        {
            await dbContext.Businesses
                           .Where(business => business.Id == id)
                           .ExecuteDeleteAsync();
            return Results.NoContent();
        });


        return group;
    }
}
