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

    public static WebApplication MapUsersEndpoints(this WebApplication app)
    {
        // GET /users
        app.MapGet("users", () =>
        {
            return Results.BadRequest();
        });

        // GET /users/1
        app.MapGet("users/{id}", async (int id, MobiMartContext dbContext) =>
        {
            User? user = await dbContext.Users.FindAsync(id);

            return user is null ? Results.NotFound() : Results.Ok(user.ToDto());
        })
        .WithName(GetUserEndpointName);

        // POST /users
        app.MapPost("users", async (CreateUserDto newUser, MobiMartContext dbContext) =>
        {
            User user = newUser.ToEntity();

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();

            return Results.CreatedAtRoute(GetUserEndpointName, new { id = user.Id }, user.ToDto());
        })
        .WithParameterValidation();

        // PUT /users/1
        app.MapPut("users/{id}", async (int id, UpdateUserDto updatedUser,MobiMartContext dbContext) =>
        {
            var existingUser = await dbContext.Users.FindAsync(id);
            if (existingUser is null) return Results.NotFound();

            dbContext.Entry(existingUser)
                .CurrentValues
                .SetValues(updatedUser.ToEntity(id));

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        // DELETE /users/1
        app.MapDelete("users/{id}", async (int id, MobiMartContext dbContext) =>
        {
            await dbContext.Users
                     .Where(game => game.Id == id)
                     .ExecuteDeleteAsync();

            return Results.NoContent();
        });

        return app;
    }
}
