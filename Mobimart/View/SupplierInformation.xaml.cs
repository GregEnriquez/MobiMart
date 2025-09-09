namespace MobiMart.View;

public partial class SupplierInformation : ContentPage
{
	public SupplierInformation()
	{
		InitializeComponent();
	}

    private async void messageSuppClick(object sender, EventArgs args)
    {
        await Shell.Current.GoToAsync(nameof(MessageSupplier));
    }

    private async void suppInventoryClick(object sender, EventArgs args)
    {
        await Shell.Current.GoToAsync(nameof(SupplierInventory));
    }
}