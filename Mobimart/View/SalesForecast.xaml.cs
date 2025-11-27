using CommunityToolkit.Maui.Extensions;
using MobiMart.ViewModel;

namespace MobiMart.View;

public partial class SalesForecast : ContentPage
{
    public SalesForecast(SalesForecastViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }



    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is SalesForecastViewModel vm)
        {
            await vm.OnAppearing();
        }
    }

    private async void OnInfoClicked(object sender, TappedEventArgs e)
    {
        if (BindingContext is SalesForecastViewModel vm && vm.Explanation != "")
        {
            var popup = new ExplanationPopup(vm.Explanation);
            this.ShowPopup(popup);
        }

    }
}