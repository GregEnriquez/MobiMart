using MobiMart.ViewModel;

namespace MobiMart.View;

public partial class Calendar : ContentPage
{
    public Calendar(CalendarViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}