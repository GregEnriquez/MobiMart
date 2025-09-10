using MobiMart.ViewModel;

namespace MobiMart.View;

public partial class SalesForecast : ContentPage
{
    public SalesForecast(SalesForecastViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}