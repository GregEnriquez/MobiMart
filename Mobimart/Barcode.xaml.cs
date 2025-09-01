namespace MobiMart;

public partial class Barcode : ContentPage
{
	public Barcode()
	{
		InitializeComponent();
	}

    private void OnHamburgerClicked(object sender, EventArgs e)
    {
        Shell.Current.FlyoutIsPresented = true;
    }
}