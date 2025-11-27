using System.Text.Json.Serialization;

namespace MobiMart.Model;

public record RevenueReport
{
    // Use `JsonPropertyName` to map C# naming conventions to the JSON keys
    [JsonPropertyName("forecastedRevenue")]
    public ForecastedRevenue ForecastedRevenue { get; init; }

    [JsonPropertyName("salesRecommendations")]
    public List<SalesRecommendation> SalesRecommendations { get; init; }

    [JsonPropertyName("requestDetails")]
    public RequestDetails RequestDetails { get; init; }
}

public record ForecastedRevenue
{
    [JsonPropertyName("amount")]
    public decimal Amount { get; init; } // Using `decimal` for financial data

    [JsonPropertyName("currency")]
    public string Currency { get; init; }

    [JsonPropertyName("description")]
    public string Description { get; init; }
}

public record SalesRecommendation
{
    [JsonPropertyName("title")]
    public string Title { get; init; }

    [JsonPropertyName("details")]
    public string Details { get; init; }
}

public record RequestDetails
{
    [JsonPropertyName("dateOfRequest")]
    public string DateOfRequest { get; init; }

    [JsonPropertyName("salesPeriod")]
    public string SalesPeriod { get; init; }
}