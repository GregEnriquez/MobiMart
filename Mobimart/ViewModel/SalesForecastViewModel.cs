using System.Diagnostics;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using Microcharts;
using MobiMart.Model;
using MobiMart.Service;
using SkiaSharp;

namespace MobiMart.ViewModel;

public partial class SalesForecastViewModel : BaseViewModel
{
    [ObservableProperty]
    string forecastedRevenue;
    [ObservableProperty]
    string currentRevenue;
    [ObservableProperty]
    string recommendation;
    [ObservableProperty]
    Chart barChart;
    [ObservableProperty]
    List<SalesRecommendation> salesRecommendations;
    [ObservableProperty]
    string explanation;

    MonthlyForecastInstance? monthlyForecast;
    bool askingAI = false, _isViewLoaded = false;

    SalesService salesService;
    InventoryService inventoryService;
    OpenAiService openAiService;
    GeminiService geminiService;
    BusinessService businessService;

    public SalesForecastViewModel(
        SalesService salesService,
        InventoryService inventoryService,
        OpenAiService openAiService,
        GeminiService geminiService,
        BusinessService businessService
        )
    {
        Title = "Sales Forecast";
        this.salesService = salesService;
        this.inventoryService = inventoryService;
        this.openAiService = openAiService;
        this.geminiService = geminiService;
        this.businessService = businessService;

        // Example placeholder data (replace later with real data or API call)
        ForecastedRevenue = "0";
        CurrentRevenue = "0";
        Recommendation = "fin.";
    }


    public async Task OnAppearing()
    {
        // // prevent Double-Loading on Tab Switches
        // // if we have already loaded data for this specific page instance, don't do it again.
        // if (_isViewLoaded) return; 
        // _isViewLoaded = true;

        await GenerateBarOverlayChart();
        Explanation = "Derived from getting the average daily sales for this month by getting total revenue so far for the current month, dividing it with the number days that has passed for the month and multiplied that to the numberof days for the next month.";
        var sales = await salesService.GetMonthlySalesRecords(DateTime.Today);

        SalesRecommendations = [];

        // check Cache if there is a record of monthly forecast for this day (limited ai usage lang kasi)
        monthlyForecast = await businessService.GetMonthlyForecastInstance();

        // only delete if it's truly old. 
        // if it contains "Error", check if that error happened RECENTLY (e.g. < 10 mins ago). 
        // if so, keep it to prevent spamming the API.
        bool shouldDelete = false;

        if (monthlyForecast != null)
        {
            // if it's from yesterday or older, delete.
            if (monthlyForecast.DateGenerated.Date < DateTimeOffset.UtcNow.Date)
            {
                shouldDelete = true;
            }
            // if it's an Error, but it's older than 10 minutes, try again. 
            // if it's a fresh error (< 10 mins), keep it to block the API call.
            else if (monthlyForecast.Response.Contains("Error") && 
                    DateTimeOffset.UtcNow.Subtract(monthlyForecast.DateGenerated).TotalMinutes > 10)
            {
                shouldDelete = true;
            }
        }

        if (shouldDelete)
        {
            await businessService.DeleteMonthlyForecastInstance();
            monthlyForecast = null;
        }

        // call gemini API
        if (monthlyForecast is null && !askingAI)
        {
            try
            {
                askingAI = true;
                monthlyForecast = await geminiService.GenerateMonthlyForecast(sales);
                
                // save to cache
                await businessService.AddMonthlyForecastInstance(monthlyForecast);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Gemini API Error: {ex.Message}");

                var businessId = Guid.Empty;
                if (Shell.Current.BindingContext is FlyoutMenuViewModel vm)
                {
                    businessId = vm.BusinessId;
                }

                // save the Error State!
                // ff we don't save this, the next OnAppearing will try again immediately.
                var errorRecord = new MonthlyForecastInstance
                {
                    BusinessId = businessId,
                    DateGenerated = DateTimeOffset.UtcNow,
                    Response = "Error: Rate Limit or Connection. Please try again later." 
                };
                
                // save this "Cooldown" record to DB
                await businessService.AddMonthlyForecastInstance(errorRecord);
                monthlyForecast = errorRecord;

                SalesRecommendations = 
                [
                    new() { Title = "Connection Issue", Details = "Could not generate AI insights. Please check back later." }
                ];
                return;
            }
            finally
            {
                askingAI = false;
            }
        }

        // deserialize (only if we have a valid JSON response)
        if (monthlyForecast != null && !monthlyForecast.Response.StartsWith("Error"))
        {
            try
            {
                string cleanJson = monthlyForecast.Response.Replace("```json", "").Replace("```", "");
                RevenueReport report = JsonSerializer.Deserialize<RevenueReport>(cleanJson)!;
                SalesRecommendations = [.. report.SalesRecommendations];

                // generate revenue report
                Explanation = report.ForecastedRevenue.Description;
                await GenerateBarOverlayChart(await _getTotalRevenue(), report.ForecastedRevenue.Amount);

            }
            catch (Exception e)
            {
                // if deserialization fails, show raw text or error
                SalesRecommendations = [ new() { Title = "Analysis Data", Details = "Data is available but could not be formatted." } ];
            }
        }
        else if (monthlyForecast != null)
        {
            // display the error message we saved
            SalesRecommendations = [ new() { Title = "Service Unavailable", Details = "AI Insights are temporarily paused." } ];
        }
    }


    private async Task<decimal> _getTotalRevenue()
    {
        var sales = await salesService.GetMonthlySalesRecords(DateTime.Today);
        if (sales is null) return 0;

        decimal totalRevenue = 0;
        foreach (var sale in sales) totalRevenue += sale.TotalPrice;

        return totalRevenue;
    }


    public async Task GenerateBarOverlayChart()
    {
        // generation of forecasted revenue using simple math
        DateTime nextMonth = DateTime.Now.AddMonths(1);
        decimal totalRevenue = await _getTotalRevenue();
        decimal currMonthAvgRevenue = totalRevenue / DateTime.Today.Day;
        decimal nextMonthRevenue = currMonthAvgRevenue * DateTime.DaysInMonth(DateTime.Now.Year, nextMonth.Month);

        /* 
        Derived from getting the average daily sales for this month by getting total revenue so far for the current month, 
        dividing it with the number days that has passed for the month and multiplied that to the number
        of days for the next month.
        */


        await GenerateBarOverlayChart(totalRevenue, nextMonthRevenue);
    }


    public async Task GenerateBarOverlayChart(decimal totalRevenue, decimal nextMonthRevenue)
    {
        DateTime nextMonth = DateTime.Now.AddMonths(1);

        CurrentRevenue = $"₱{totalRevenue:0.00}";
        ForecastedRevenue = $"₱{nextMonthRevenue:0.00}";

        // actual generation of chart
        var entries = new List<ChartEntry>
        {
            new((float)totalRevenue)
            {
                Label = $"~{DateTime.Now:M}",
                ValueLabel = CurrentRevenue,
                Color = SKColor.Parse("#83D1E3")
            },
            new((float)nextMonthRevenue)
            {
                Label = $"{new DateTime(nextMonth.Year, nextMonth.Month, DateTime.DaysInMonth(nextMonth.Year, nextMonth.Month)):M}",
                ValueLabel = ForecastedRevenue,
                Color = SKColor.Parse("#9FEEF0")
            }
        };
        decimal maxValue = totalRevenue;
        if (nextMonthRevenue > maxValue) maxValue = nextMonthRevenue;
        BarChart = new BarChart()
        {
            Entries = entries,
            LabelTextSize = 32f,
            ValueLabelTextSize = 32f,
            ValueLabelOption = ValueLabelOption.OverElement,
            LabelOrientation = Orientation.Horizontal,
            ValueLabelOrientation = Orientation.Horizontal,
            BackgroundColor = SKColor.Parse("#FFFBE9"),
            ShowYAxisText = true,
            ShowYAxisLines = true,
            YAxisPosition = Position.Left,
            MinValue = 0,
            MaxValue = (float)(maxValue + 500),
        };
    }


    private static SKColor GetRandomColor(bool isOpaque = false)
    {
        Random _random = new();
        // Generate random values for Red, Green, Blue, and Alpha (0-255)
        byte red = (byte)_random.Next(256);
        byte green = (byte)_random.Next(256);
        byte blue = (byte)_random.Next(256);
        byte alpha = (byte)_random.Next(256);

        // Create a new Color object from the random components
        return isOpaque ? new SKColor(red, green, blue, 255) : new SKColor(red, green, blue, alpha);
        // return SKColor.Parse(Color.FromRgba(red, green, blue, alpha).ToRgbaString());
    }
}
