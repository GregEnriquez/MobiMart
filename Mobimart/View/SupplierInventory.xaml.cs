namespace MobiMart.View;

public partial class SupplierInventory : ContentPage
{
	public SupplierInventory()
	{
		InitializeComponent();
	}

    private void OnHamburgerClicked(object sender, EventArgs e)
    {
        Shell.Current.FlyoutIsPresented = true;
    }

    private async void editTapped(object sender, EventArgs args)
    {
        await Shell.Current.GoToAsync(nameof(EditSupplierInventory), true);
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}