using MobiMart.ViewModel;

namespace MobiMart.View;

public partial class EmployeeTablePage : ContentPage
{
    public EmployeeTablePage(EmployeeTablePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        Routing.RegisterRoute("EmployeeTablePage", typeof(EmployeeTablePage));
    }

    private void OnHamburgerClicked(object sender, EventArgs e)
    {
        Shell.Current.FlyoutIsPresented = true;
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
    
    protected override void OnAppearing()
	{
		base.OnAppearing();

		if (BindingContext is EmployeeTablePageViewModel vm)
		{
			vm.RefreshEmployees();
		}
	}
}