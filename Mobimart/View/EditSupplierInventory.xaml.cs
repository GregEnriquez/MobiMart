using MobiMart.Model;
using MobiMart.ViewModel;

namespace MobiMart.View;

public partial class EditSupplierInventory : ContentPage, IQueryAttributable
{
	private EditSuppInventoryViewModel vm = new();
	public EditSupplierInventory()
	{
		InitializeComponent();
		BindingContext = vm;
	}

	public void ApplyQueryAttributes(IDictionary<string, object> query)
	{
		if (query.TryGetValue("InventoryItem", out var item) && item is WholesaleInventory inv)
		{
			vm.loadItems(inv);
		} 
	}
}