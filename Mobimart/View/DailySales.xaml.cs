using MobiMart.ViewModel;

namespace MobiMart.View;

public partial class DailySales : ContentPage
{
    public DailySales(DailySalesViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is DailySalesViewModel vm)
        {
            await vm.OnAppearing();
        }
    }
}