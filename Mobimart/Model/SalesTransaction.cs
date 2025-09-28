using System;
using SQLite;

namespace MobiMart.Model;

public class SalesTransaction
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Date { get; set; } = "";
    public float TotalPrice { get; set; }
    public float Payment { get; set; }
    public float Change { get; set; }
}
