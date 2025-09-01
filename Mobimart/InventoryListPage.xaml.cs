namespace MobiMart;

public partial class InventoryListPage : ContentPage
{
	public InventoryListPage()
	{
		InitializeComponent();
	}

    private void OnHamburgerClicked(object sender, EventArgs e)
    {
        Shell.Current.FlyoutIsPresented = true;
    }
}