using System.ComponentModel.DataAnnotations;
namespace MobiMart.Api.Dtos;

public record class UserDto(
    int Id,
    int? BusinessId,
    string FirstName,
    string LastName,
    string Email,
    string Password,
    DateOnly BirthDate,
    int Age,
    string PhoneNumber,
    string EmployeeType
);

public record class UpdateUserDto(
    int BusinessId,
    [Required][StringLength(50)] string FirstName,
    [Required][StringLength(50)] string LastName,
    [Required] string Email,
    [Required][MinLength(8)] string Password,
    DateOnly BirthDate,
    int Age,
    string PhoneNumber,
    string EmployeeType
);

public record class CreateUserDto(
    int BusinessId,
    [Required][StringLength(50)] string FirstName,
    [Required][StringLength(50)] string LastName,
    [Required] string Email,
    [Required][MinLength(8)] string Password,
    DateOnly BirthDate,
    int Age,
    string PhoneNumber,
    string EmployeeType
);