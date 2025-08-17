using System;

namespace MobiMart.Api.Entities;

public class User
{
    public int Id { get; set; }
    public int? BusinessRefId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public DateOnly BirthDate { get; set; }
    public int Age { get; set; }
    public string PhoneNumber { get; set; } = "";
    public required string EmployeeType { get; set; }
}
