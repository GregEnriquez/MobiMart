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
