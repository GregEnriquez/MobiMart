namespace MobiMart.View;

public partial class SupplierInventory : ContentPage
{
	public SupplierInventory()
	{
		InitializeComponent();
	}

	private async void editTapped(object sender, EventArgs args)
	{
		await Shell.Current.GoToAsync(nameof(EditSupplierInventory));
	}
}