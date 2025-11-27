using System;
using SQLite;

namespace MobiMart.Model;

public class User : SyncEntity
{
    [Indexed]
    public Guid BusinessId { get; set; }

    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public int PasswordLength { get; set; }

    public DateTime BirthDate { get; set; } // DateOnly
    public int Age { get; set; }
    public string PhoneNumber { get; set; } = "";
    public string EmployeeType { get; set; } = "";
    
    public string RefreshToken { get; set; } = "";
    public DateTimeOffset? RefreshTokenExpiryTime { get; set; } //DateTime


    public void UpdateFromUserObject(User pulledUser)
    {
        Id = pulledUser.Id;
        LastUpdatedAt = pulledUser.LastUpdatedAt;
        IsDeleted = pulledUser.IsDeleted;
        BusinessId = pulledUser.BusinessId;
        FirstName = pulledUser.FirstName;
        LastName = pulledUser.LastName;
        Email = pulledUser.Email;
        Password = pulledUser.Password;
        PasswordLength = pulledUser.PasswordLength;
        BirthDate = pulledUser.BirthDate;
        Age = pulledUser.Age;
        PhoneNumber = pulledUser.PhoneNumber;
        EmployeeType = pulledUser.EmployeeType;
    }
}
