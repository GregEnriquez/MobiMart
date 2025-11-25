using System;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using MobiMart.Model;
using MobiMart.ViewModel;

namespace MobiMart.Service;

public class GeminiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public GeminiService()
    {
        _apiKey = "AIzaSyBcbOjXmhl-NVRdftwPlwLPAMY5D5USGy8";
        _httpClient = new HttpClient();
    }

    public async Task<string> GenerateContentAsync(string prompt)
    {
        var url = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent";

        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new { text = prompt }
                    }
                }
            }
        };

        var jsonPayload = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        // Add the API key as a header, as required by the Gemini API.
        _httpClient.DefaultRequestHeaders.Add("x-goog-api-key", _apiKey);

        var response = await _httpClient.PostAsync(url, content);

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var geminiResponse = JsonSerializer.Deserialize<GeminiResponse>(jsonResponse);

            // Extract the generated text from the JSON response
            return geminiResponse?.candidates[0].content.parts[0].text ?? "No response.";
        }
        else
        {
            var errorResponse = await response.Content.ReadAsStringAsync();
            return $"Error: {response.ReasonPhrase} - {errorResponse}";
        }
    }



    public async Task<MonthlyForecastInstance> GenerateMonthlyForecast(List<SalesRecord> monthlySales)
    {
        // generate prompt
            string prompt = """
            As an expert sales analyst, your task is to provide a sales forecast and actionable sales forecasting recommendations for the upcoming month.
            Here is the sales data for the current month, presented as a list of SalesRecords. Each record includes the transaction details and a list of items sold.
            **SalesRecords Data for the Current Month:**

            """;
            string jsonString = JsonSerializer.Serialize(monthlySales);
            prompt += jsonString;
            prompt += $"""
            Task Requirements:
            1. Forecasted Revenue: Provide a forecasted total revenue for the next month based on the provided sales data. If specific numbers are difficult to generate from a single month's data, provide qualitative insights about potential trends.

            2. Sales Forecasting Recommendations: Generate at least two specific, actionable recommendations tailored for the upcoming month. These recommendations should consider potential seasonal events, market trends, or patterns observed in the provided sales data. Focus on product categories or sales strategies. The recommendations should be formatted similarly to this example:"Sales Forecasting Recommendations:
            Start of new School Year: This month is the usual start of a new school year. Sales regarding school supplies or snacks will be on the Rise." 

            Consider the current date for seasonal analysis:{DateTime.Now:D}
            """;
        prompt += """
            Provide your answer in JSON format as well. Here is the template:

            {
            "forecastedRevenue": {
                "amount": "[Insert Forecasted Revenue Amount, e.g., 65000.50]",
                "currency": "[Insert Currency, e.g., \"PHP\"]",
                "description": "[Insert a brief explanation of how the forecast was derived, referencing historical data, seasonal trends, and current sales performance. You can also mention any limitations of the forecast, such as being based on only one month's data.]"
            },
            "salesRecommendations": [
                {
                "title": "[Insert Title of Recommendation, e.g., \"Start of New School Year\"]",
                "details": "[Insert a sentence or two explaining the recommendation, e.g., \"This month is the usual start of a new school year. Sales regarding school supplies or snacks will be on the rise.\"]"
                },
                {
                "title": "[Insert Title of Second Recommendation, e.g., \"Seasonal Holiday Push\"]",
                "details": "[Insert a sentence or two explaining the recommendation, e.g., \"The upcoming holiday season is a key sales period. Focus marketing efforts on popular gift items and bundles.\"]"
                },
                {
                "title": "[Insert Title of Third Recommendation, if applicable]",
                "details": "[Insert a sentence or two explaining the recommendation, if applicable]"
                }
            ],
            "requestDetails": {
                "dateOfRequest": "2025-10-10",
                "salesPeriod": "September 2025"
            }
            }
            """;

        // prompt gemini
        string response = await GenerateContentAsync(prompt);

        // return response
        var businessId = Guid.Empty;
        if (Shell.Current.BindingContext is FlyoutMenuViewModel vm)
        {
            businessId = vm.BusinessId;
        }
        return new MonthlyForecastInstance()
        {
            BusinessId = businessId,
            Response = response,
            DateGenerated = DateTimeOffset.UtcNow
        };
    }
}


// Define data models for deserializing the JSON response
public class GeminiResponse
{
    public Candidate[] candidates { get; set; }
}

public class Candidate
{
    public Content content { get; set; }
    public string finishReason { get; set; }
    public int index { get; set; }
}

public class Content
{
    public Part[] parts { get; set; }
    public string role { get; set; }
}

public class Part
{
    public string text { get; set; }
}

