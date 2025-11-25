using System;
using SQLite;

namespace MobiMart.Model;

public class User : SyncEntity
{
    [Indexed]
    public Guid BusinessRefId { get; set; }

    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";

    public DateTime? BirthDate { get; set; } // DateOnly
    public int Age { get; set; }
    public string PhoneNumber { get; set; } = "";
    public string EmployeeType { get; set; } = "";
    
    public string RefreshToken { get; set; } = "";
    public DateTimeOffset? RefreshTokenExpiryTime { get; set; } //DateTime
}
