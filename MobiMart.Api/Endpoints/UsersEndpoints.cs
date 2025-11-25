using System;
using Microsoft.EntityFrameworkCore;
using MobiMart.Api.Data;
using MobiMart.Api.Dtos;
using MobiMart.Api.Entities;
using MobiMart.Api.Mapping;

namespace MobiMart.Api.Endpoints;

public static class UsersEndpoints
{
    const string GetUserEndpointName = "GetUser";

    public static RouteGroupBuilder MapUsersEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("users");

        // GET /users
        group.MapGet("/", (MobiMartContext dbContext) =>
        {
            return Results.Ok(dbContext.Users.Find(1));
            // return Results.BadRequest();
        });

        // GET /users/id (GUID)
        group.MapGet("/{id:guid}", async (Guid id, MobiMartContext dbContext) =>
        {
            User? user = await dbContext.Users.FindAsync(id);
            return user is null ? Results.NotFound() : Results.Ok(user.ToDto());
        });

        // GET /users/email@sample.com
        group.MapGet("/{email}", async (string email, MobiMartContext dbContext) =>
        {
            User? user = await dbContext.Users.FirstOrDefaultAsync(x => x.Email == email);

            return user is null ? Results.NotFound() : Results.Ok(user.ToDto());
        })
        .WithName(GetUserEndpointName);

        // POST /users
        group.MapPost("/", async (CreateUserDto newUser, MobiMartContext dbContext) =>
        {
            User user = newUser.ToEntity();

            user.LastUpdatedAt = DateTimeOffset.UtcNow;

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();

            return Results.CreatedAtRoute(GetUserEndpointName, new { id = user.Id }, user.ToDto());
        })
        .WithParameterValidation();

        // PUT /users/{guid}
        group.MapPut("/{id:guid}", async (Guid id, UpdateUserDto updatedUser,MobiMartContext dbContext) =>
        {
            var existingUser = await dbContext.Users.FindAsync(id);
            if (existingUser is null) return Results.NotFound();

            var newValues = updatedUser.ToEntity(id);
            newValues.LastUpdatedAt = DateTimeOffset.UtcNow; // Update Timestamp

            dbContext.Entry(existingUser)
                .CurrentValues
                .SetValues(newValues);

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        // DELETE /users/{guid}
        group.MapDelete("/{id:guid}", async (Guid id, MobiMartContext dbContext) =>
        {
            // await dbContext.Users
            //          .Where(game => game.Id == id)
            //          .ExecuteDeleteAsync();

            // soft delete
            var rowsAffected = await dbContext.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(u => u.IsDeleted, true)
                    .SetProperty(u => u.LastUpdatedAt, DateTimeOffset.UtcNow)
                );

            return rowsAffected > 0 ? Results.NoContent() : Results.NotFound();
        });

        return group;
    }
}
