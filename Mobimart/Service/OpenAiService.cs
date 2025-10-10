using System;
using System.ClientModel;
using System.Diagnostics;
using System.Text.Json;
using MobiMart.Model;
using OpenAI;
using OpenAI.Chat;

namespace MobiMart.Service;

public class OpenAiService
{
    // lmao. remove this in production
    private readonly string API = "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";
    private OpenAIClient chatGptClient;

    public OpenAiService()
    {
        chatGptClient = new(API);
    }


    public async Task GenerateMonthlyForecast(List<SalesRecord> monthlySales)
    {
        try
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

            // prompt gpt
            var client = chatGptClient.GetChatClient("gpt-3.5-turbo-16k");
            AsyncCollectionResult<StreamingChatCompletionUpdate> updates = client.CompleteChatStreamingAsync(prompt);

            // serialize json response to dictionary
            StringWriter responseWriter = new();

            await foreach (StreamingChatCompletionUpdate update in updates)
            {
                foreach (ChatMessageContentPart updatePart in update.ContentUpdate)
                {
                    responseWriter.Write(updatePart.Text);
                }
            }
            var returnMessage = responseWriter.ToString();

            // return the serialized json response
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.StackTrace);
        }
    }
    
}