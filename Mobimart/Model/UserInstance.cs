using System;

namespace MobiMart.Model;

public class UserInstance
{
    public int UserId { get; set; }
    public string AccessToken { get; set; } = "";
    public string RefreshToken { get; set; } = "";
    public string RefreshTokenExpiryTime { get; set; } = "";
}
