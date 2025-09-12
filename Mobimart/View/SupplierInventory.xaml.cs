using MobiMart.Model;
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
		if ( sender is Element element && element.BindingContext is WholesaleInventory item)
		{
			var navParam = new Dictionary<string, object>
			{
				{ "InventoryItem", item }
			};

            await Shell.Current.GoToAsync(nameof(EditSupplierInventory), navParam);
        }
	}

	private async void addTapped(object sender, EventArgs args)
	{
        await Shell.Current.GoToAsync(nameof(AddSupplierItem));
    }
}