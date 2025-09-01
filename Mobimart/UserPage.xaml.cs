namespace MobiMart;

public partial class UserPage : ContentPage
{
	public UserPage()
	{
		InitializeComponent();
	}

    private void OnHamburgerClicked(object sender, EventArgs e)
    {
        Shell.Current.FlyoutIsPresented = true;
    }

    private async void OnCreateClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(BusinessPage), true);
    }

}