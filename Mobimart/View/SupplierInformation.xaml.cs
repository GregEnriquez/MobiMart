namespace MobiMart.View;

public partial class SupplierInformation : ContentPage
{
	public SupplierInformation()
	{
		InitializeComponent();
	}

    private void OnHamburgerClicked(object sender, EventArgs e)
    {
        Shell.Current.FlyoutIsPresented = true;
    }

    private async void messageSuppClick(object sender, EventArgs args)
    {
        await Shell.Current.GoToAsync(nameof(MessageSupplier), true);
    }

    private async void suppInventoryClick(object sender, EventArgs args)
    {
        await Shell.Current.GoToAsync(nameof(SupplierInventory), true);
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}