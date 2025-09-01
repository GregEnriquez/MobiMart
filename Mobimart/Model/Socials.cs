using System;
using SQLite;

namespace MobiMart.Model;

public class Socials
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Link { get; set; } = "";
    public string Description { get; set; } = "";
    public required string LastModified { get; set; }
}
