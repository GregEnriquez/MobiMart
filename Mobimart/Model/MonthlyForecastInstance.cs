using System;
using System.ComponentModel.DataAnnotations.Schema;
using SQLite;

namespace MobiMart.Model;

public class MonthlyForecastInstance
{
    [PrimaryKey]
    public int Id { get; set; }
    [ForeignKey(nameof(Business))]
    public int BusinessId { get; set; }
    public string Response { get; set; } = "";
    public string DateGenerated { get; set; } = "";
}
