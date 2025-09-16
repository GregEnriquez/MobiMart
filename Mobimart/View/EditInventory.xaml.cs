using MobiMart.ViewModel;
using MobiMart.Model;

namespace MobiMart.View;

public partial class EditInventory : ContentPage
{
    
    public EditInventory(Inventory item)
	{
		InitializeComponent();
        BindingContext = new EditInventoryViewModel(item);
    }
}