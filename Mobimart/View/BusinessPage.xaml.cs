using MobiMart.ViewModel;

namespace MobiMart.View;

public partial class BusinessPage : ContentPage
{
	public BusinessPage(BusinessPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();

		if (BindingContext is BusinessPageViewModel vm)
		{
			vm.UpdateInfo();
		}
	}


	public async void onViewEmployeesClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(EmployeeTablePage));
	}
}