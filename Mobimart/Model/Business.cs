using System;
using SQLite;

namespace MobiMart.Model;

public class Business
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Code { get; set; }
    public string LastModified { get; set; }
}
