using System;
using SQLite;

namespace MobiMart.Model;

public class UserInstance
{
    [PrimaryKey]
    public int Id { get; set; }
    public int UserId { get; set; }
    public string AccessToken { get; set; } = "";
    public string RefreshToken { get; set; } = "";
    public string RefreshTokenExpiryTime { get; set; } = "";
}
