using MobiMart.ViewModel;

namespace MobiMart.View;

public partial class DailySales : ContentPage
{
    public DailySales(DailySalesViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}