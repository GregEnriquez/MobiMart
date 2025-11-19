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

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		// ask for permissions
		if (OperatingSystem.IsAndroidVersionAtLeast(33))
		{
			var status = await Permissions.CheckStatusAsync<Permissions.PostNotifications>();
			if (status != PermissionStatus.Granted)
				await Permissions.RequestAsync<Permissions.PostNotifications>();
		}
		
		if (BindingContext is UserPageViewModel vm)
		{
			// If the MainPage is NOT the AppShell, it means we are transitioning from logout to login.
			if (Application.Current!.Windows[0].Page is not AppShell) return;

			await vm.UpdateInfo();
		}
    }
}