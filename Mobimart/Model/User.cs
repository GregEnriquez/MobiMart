using System;
using SQLite;

namespace MobiMart.Model;

public class User
{
    [PrimaryKey]
    public int Id { get; set; }
    [Indexed]
    public int BusinessRefId { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public string BirthDate { get; set; } = ""; // DateOnly
    public int Age { get; set; }
    public string PhoneNumber { get; set; } = "";
    public string EmployeeType { get; set; } = "";
    public string RefreshToken { get; set; } = "";
    public string RefreshTokenExpiryTime { get; set; } = ""; //DateTime
    public string LastModified { get; set; } = "";
}
