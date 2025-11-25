using System.ComponentModel.DataAnnotations;
namespace MobiMart.Api.Dtos;

public record class UserDto(
    Guid Id,
    Guid BusinessId,
    string FirstName,
    string LastName,
    string Email,
    string Password,
    int PasswordLength, // plaintext password' length not the hashed one
    DateTime BirthDate,
    int Age,
    string PhoneNumber,
    string EmployeeType,
    DateTimeOffset LastUpdatedAt,
    bool IsDeleted
);

public record class UpdateUserDto(
    Guid BusinessId,
    [Required][StringLength(50)] string FirstName,
    [Required][StringLength(50)] string LastName,
    [Required] string Email,
    [Required][MinLength(8)] string Password,
    [Required] int PasswordLength,
    DateTime BirthDate,
    int Age,
    string PhoneNumber,
    string EmployeeType
);

public record class CreateUserDto(
    Guid Id,
    Guid BusinessId,
    [Required][StringLength(50)] string FirstName,
    [Required][StringLength(50)] string LastName,
    [Required] string Email,
    [Required][MinLength(8)] string Password,
    [Required] int PasswordLength,
    DateTime BirthDate,
    int Age,
    string PhoneNumber,
    string EmployeeType,
    bool IsDeleted,
    DateTimeOffset LastUpdatedAt
);


public record class UserAuthDto(
    [Required] string Email,
    [Required] string Password
);


public record class TokenResponseDto(
    [Required] string AccessToken,
    [Required] string RefreshToken
);


public record class RefreshTokenRequestDto(
    Guid UserId,
    string RefreshToken
);


public record class LogoutUserDto(
    Guid UserId
);