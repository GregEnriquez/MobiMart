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

    MonthlyForecastInstance? monthlyForecast;

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
        ForecastedRevenue = "₱50,000";
        CurrentRevenue = "₱32,500";
        Recommendation = "Start of new School Year: Expect higher demand for school supplies and snacks.";
    }



    public async Task OnAppearing()
    {
        await GenerateBarOverlayChart();
        var sales = await salesService.GetMonthlySalesRecords(DateTime.Today);
        // await openAiService.GenerateMonthlyForecast(sales);

        // get monthly forecast for today using gemini service
        SalesRecommendations = [];
        // await businessService.DeleteMonthlyForecastInstance(); //debug
        monthlyForecast = await businessService.GetMonthlyForecastInstance();
        if (monthlyForecast is not null 
        && DateTime.Parse(monthlyForecast.DateGenerated).Date < DateTime.Now.Date)
        {
            await businessService.DeleteMonthlyForecastInstance();
            monthlyForecast = null;
        }
        if (monthlyForecast is null)
        {
            monthlyForecast = await geminiService.GenerateMonthlyForecast(sales);
            await businessService.AddMonthlyForecastInstance(monthlyForecast);
        }
        // convert the response revenue report
        try
        {
            string og = monthlyForecast.Response;
            monthlyForecast.Response = monthlyForecast.Response.Replace("```json", "");
            monthlyForecast.Response = monthlyForecast.Response.Replace("```", "");
            RevenueReport report = JsonSerializer.Deserialize<RevenueReport>(monthlyForecast.Response)!;
            SalesRecommendations = [.. report.SalesRecommendations.AsEnumerable()];
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.StackTrace);
        }
    }



    public async Task GenerateBarOverlayChart()
    {
        DateTime nextMonth = DateTime.Now.AddMonths(1);
        // generation of data
        var sales = await salesService.GetMonthlySalesRecords(DateTime.Today);
        float totalRevenue = 0;
        if (sales is null) return;

        foreach (var sale in sales) totalRevenue += sale.TotalPrice;

        CurrentRevenue = $"₱{totalRevenue:0.00}";
        float currMonthAvgRevenue = totalRevenue / DateTime.Today.Day;
        float nextMonthRevenue = currMonthAvgRevenue * DateTime.DaysInMonth(DateTime.Now.Year, nextMonth.Month);
        ForecastedRevenue = $"₱{nextMonthRevenue:0.00}";

        // actual generation of chart
        var entries = new List<ChartEntry>
        {
            new(totalRevenue)
            {
                Label = $"~{DateTime.Now:M}",
                ValueLabel = CurrentRevenue,
                Color = SKColor.Parse("#83D1E3")
            },
            new(nextMonthRevenue)
            {
                Label = $"{new DateTime(nextMonth.Year, nextMonth.Month, DateTime.DaysInMonth(nextMonth.Year, nextMonth.Month)):M}",
                ValueLabel = ForecastedRevenue,
                Color = SKColor.Parse("#9FEEF0")
            }
        };
        float maxValue = totalRevenue;
        if (nextMonthRevenue > maxValue) maxValue = nextMonthRevenue;
        BarChart = new BarChart()
        {
            Entries = entries,
            LabelTextSize = 46f,
            ValueLabelTextSize = 46f,
            ValueLabelOption = ValueLabelOption.OverElement,
            LabelOrientation = Orientation.Horizontal,
            ValueLabelOrientation = Orientation.Horizontal,
            BackgroundColor = SKColor.Parse("#FFFBE9"),
            ShowYAxisText = true,
            ShowYAxisLines = true,
            YAxisPosition = Position.Left,
            MinValue = 0,
            MaxValue = (maxValue + 500),
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
