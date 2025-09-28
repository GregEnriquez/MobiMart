using MobiMart.Model;
using MobiMart.ViewModel;

namespace MobiMart.View;


[QueryProperty(nameof(DeliveryId), "DeliveryId")]
public partial class EditSupplierInventory : ContentPage
{

	private int _deliveryId;
	public int DeliveryId
	{
		get => _deliveryId;
		set
		{
			_deliveryId = value;
			if (BindingContext is EditSuppInventoryViewModel vm)
			{
				vm.DeliveryId = value;
			}
		}
	}

	public EditSupplierInventory(EditSuppInventoryViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}


	protected override async void OnAppearing()
	{
		base.OnAppearing();

		if (BindingContext is EditSuppInventoryViewModel vm)
		{
			await vm.OnAppearing();
		}
    }
}