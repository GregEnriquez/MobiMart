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

	private async void editTapped(object sender, TappedEventArgs args)
	{
		var navParam = new Dictionary<string, object>
		{
			{ "DeliveryId", args.Parameter! },
			{ "IsEditDelivery", true}
		};

		await Shell.Current.GoToAsync(nameof(EditSupplierInventory), true, navParam);
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