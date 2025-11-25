using System;
using System.ComponentModel.DataAnnotations.Schema;
using SQLite;

namespace MobiMart.Model;

public class MonthlyForecastInstance : SyncEntity
{
    [Indexed]
    public Guid BusinessId { get; set; }
    public string Response { get; set; } = "";
    public DateTimeOffset DateGenerated { get; set; }
}
