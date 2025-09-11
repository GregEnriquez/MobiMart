using MobiMart.ViewModel;

namespace MobiMart.View;

public partial class UserPage : ContentPage
{
	public UserPage(UserPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	private async void onCreateClicked(object sender, EventArgs args)
	{
		await Shell.Current.GoToAsync("//BusinessPage", true);
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		
		if (BindingContext is UserPageViewModel vm)
		{
			vm.UpdateInfo();
		}
    }
}