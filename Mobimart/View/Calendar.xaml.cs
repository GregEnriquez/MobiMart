using MobiMart.ViewModel;

namespace MobiMart.View;

public partial class Calendar : ContentPage
{
    public Calendar(CalendarViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is CalendarViewModel vm)
        {
            await vm.OnAppearing();
        }
    }
}