using CommunityToolkit.Mvvm.ComponentModel;

namespace MobiMart.ViewModel;

public partial class SalesForecastViewModel : BaseViewModel
{
    [ObservableProperty]
    string forecastedRevenue;

    [ObservableProperty]
    string currentRevenue;

    [ObservableProperty]
    string recommendation;

    public SalesForecastViewModel()
    {
        Title = "Sales Forecast";

        // Example placeholder data (replace later with real data or API call)
        ForecastedRevenue = "₱50,000";
        CurrentRevenue = "₱32,500";
        Recommendation = "Start of new School Year: Expect higher demand for school supplies and snacks.";
    }
}
