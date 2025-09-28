using CommunityToolkit.Mvvm.ComponentModel;
using MobiMart.Model;
using MobiMart.ViewModel;

namespace MobiMart.View;

[QueryProperty(nameof(Supplier), "Supplier")]
public partial class SupplierInventory : ContentPage
{
	private Supplier? supplier;
	public Supplier? Supplier
	{
		get => supplier;
		set
		{
			supplier = value;
			if (BindingContext is SupplierInventoryViewModel vm)
			{
				vm.Supplier = value!;
			}
		}
	}

	public SupplierInventory(SupplierInventoryViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	private async void editTapped(object sender, EventArgs args)
	{
		if (sender is Element element && element.BindingContext is WholesaleInventory item)
		{
			var navParam = new Dictionary<string, object>
			{
				{ "DeliveryId", 1 }
			};

			await Shell.Current.GoToAsync(nameof(EditSupplierInventory), navParam);
		}
	}

	private async void addTapped(object sender, EventArgs args)
	{
		await Shell.Current.GoToAsync(nameof(AddSupplierItem));
	}



	protected override async void OnAppearing()
	{
		base.OnAppearing();


		if (BindingContext is SupplierInventoryViewModel vm)
		{
			await vm.RefreshRecords();
		}
    }
}