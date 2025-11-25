using System;
using SQLite;

namespace MobiMart.Model;

public class Business : SyncEntity
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string Code { get; set; }
}
