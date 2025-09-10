using MobiMart.ViewModel;

namespace MobiMart.View;

public partial class IncomeSummary : ContentPage
{
    public IncomeSummary(IncomeSummaryViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}