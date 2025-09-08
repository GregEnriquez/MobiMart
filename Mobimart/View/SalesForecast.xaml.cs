namespace MobiMart.View;

public partial class SalesForecast : ContentPage
{
	public SalesForecast()
	{
		InitializeComponent();
	}

    private void OnHamburgerClicked(object sender, EventArgs e)
    {
        Shell.Current.FlyoutIsPresented = true;
    }
}