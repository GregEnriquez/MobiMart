using System;

namespace MobiMart.Api.Entities;

public class User : SyncEntity
{
    public Guid BusinessId { get; set; }

    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public int PasswordLength { get; set; }

    public DateTime BirthDate { get; set; }
    public int Age { get; set; }
    public string PhoneNumber { get; set; } = "";
    public required string EmployeeType { get; set; }

    public string? RefreshToken { get; set; }
    public DateTimeOffset? RefreshTokenExpiryTime { get; set; }
}
