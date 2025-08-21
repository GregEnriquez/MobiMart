using System;
using SQLite;

namespace MobiMart.Model;

public class User
{
    [AutoIncrement, PrimaryKey]
    public int Id { get; set; }
    public int? BusinessRefId { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public DateOnly BirthDate { get; set; }
    public int Age { get; set; }
    public string PhoneNumber { get; set; } = "";
    public string EmployeeType { get; set; } = "";
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
}
