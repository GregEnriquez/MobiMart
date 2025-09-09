namespace MobiMart.View;

public partial class ViewTransaction : ContentPage
{
	public ViewTransaction()
	{
		InitializeComponent();
	}

    private void OnHamburgerClicked(object sender, EventArgs e)
    {
        Shell.Current.FlyoutIsPresented = true;
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}