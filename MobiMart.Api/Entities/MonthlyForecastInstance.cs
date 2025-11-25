using System;

namespace MobiMart.Api.Entities;

public class MonthlyForecastInstance : SyncEntity
{
    public Guid BusinessId { get; set; }
    
    public string Response { get; set; } = "";
    public DateTimeOffset DateGenerated { get; set; }
}
