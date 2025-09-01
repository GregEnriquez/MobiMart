using System;
using SQLite;

namespace MobiMart.Model;

public class Business
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required string Code { get; set; }
    public required string LastModified { get; set; }
}
