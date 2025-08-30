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

        // GET /users/1
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

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();

            return Results.CreatedAtRoute(GetUserEndpointName, new { id = user.Id }, user.ToDto());
        })
        .WithParameterValidation();

        // PUT /users/1
        group.MapPut("/{id}", async (int id, UpdateUserDto updatedUser,MobiMartContext dbContext) =>
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
        group.MapDelete("/{id}", async (int id, MobiMartContext dbContext) =>
        {
            await dbContext.Users
                     .Where(game => game.Id == id)
                     .ExecuteDeleteAsync();

            return Results.NoContent();
        });

        return group;
    }
}
