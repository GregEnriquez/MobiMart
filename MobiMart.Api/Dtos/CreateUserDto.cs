using System.ComponentModel.DataAnnotations;

namespace MobiMart.Api.Dtos;

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
