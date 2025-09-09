namespace MobiMart.View;

public partial class SupplierList : ContentPage
{
	public SupplierList()
	{
		InitializeComponent();
	}

    private void OnHamburgerClicked(object sender, EventArgs e)
    {
        Shell.Current.FlyoutIsPresented = true;
    }

    private async void onSupplierTap(object sender, EventArgs args)
    {
        await Shell.Current.GoToAsync(nameof(SupplierInformation), true);
    }

    private async void onButtonClicked(object sender, EventArgs args)
    {
        await Shell.Current.GoToAsync(nameof(AddSupplier), true);
    }
}