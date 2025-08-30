using System;
using MobiMart.Api.Dtos;
using MobiMart.Api.Entities;

namespace MobiMart.Api.Services;

public interface IAuthService
{
    Task<User?> RegisterAsync(UserAuthDto request);
    Task<TokenResponseDto?> LoginAsync(UserAuthDto request);
    Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request);
    Task<bool> LogoutAsync(int id);
}
