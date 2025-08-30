using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MobiMart.Api.Data;
using MobiMart.Api.Dtos;
using MobiMart.Api.Entities;
using MobiMart.Api.Mapping;

namespace MobiMart.Api.Services;

public class AuthService(MobiMartContext context, IConfiguration configuration) : IAuthService
{
    public async Task<TokenResponseDto?> LoginAsync(UserAuthDto request)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user is null)
            return null;

        if (new PasswordHasher<User>().VerifyHashedPassword(user, user.Password, request.Password) == PasswordVerificationResult.Failed)
        {
            return null;
        }

        return await CreateTokenResponse(user);
    }

    public async Task<User?> RegisterAsync(UserAuthDto request)
    {
        if (await context.Users.AnyAsync(u => u.Email == request.Email)) return null;

        var user = new User()
        {
            FirstName = "",
            LastName = "",
            Email = "",
            Password = "",
            EmployeeType = ""
        };
        var hashedPassword = new PasswordHasher<User>().HashPassword(user, request.Password);

        user.Email = request.Email;
        user.Password = hashedPassword;

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return user;
    }


    private async Task<TokenResponseDto> CreateTokenResponse(User? user)
    {
        return new TokenResponseDto(
            CreateToken(user!),
            await GenerateAndSaveRefreshTokenAsync(user!)
        );
    }

    private string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Email),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Role, user.EmployeeType)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration.GetValue<String>("AppSettings:Token")!)
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: configuration.GetValue<String>("AppSettings:Issuer"),
            audience: configuration.GetValue<String>("AppSettings:Audience"),
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }


    private async Task<String> GenerateAndSaveRefreshTokenAsync(User user)
    {
        var refreshToken = GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMonths(1);
        await context.SaveChangesAsync();
        return refreshToken;
    }


    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }


    public async Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request)
    {
        var user = await ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);
        if (user is null)
            return null;

        return await CreateTokenResponse(user);
    }
    
    
    private async Task<User?> ValidateRefreshTokenAsync(int userId, string refreshToken)
    {
        var user = await context.Users.FindAsync(userId);
        if (user is null || user.RefreshToken != refreshToken
            || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return null;
        }

        return user;
    }

    public async Task<bool> LogoutAsync(int userId)
    {
        var user = await context.Users.FindAsync(userId);
        if (user is null) return false;

        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = null;
        await context.SaveChangesAsync();

        return true;
    }
}
