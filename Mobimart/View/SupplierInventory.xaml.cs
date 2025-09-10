using MobiMart.ViewModel;

namespace MobiMart.View;

public partial class SupplierInventory : ContentPage
{
	public SupplierInventory()
	{
		InitializeComponent();
		BindingContext = new SupplierInventoryViewModel();
	}

	private async void editTapped(object sender, EventArgs args)
	{
		await Shell.Current.GoToAsync(nameof(EditSupplierInventory));
	}
}