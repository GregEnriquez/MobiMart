using CommunityToolkit.Maui.Views;
using MobiMart.ViewModel;

namespace MobiMart.View;

public partial class EditInventoryPopup : Popup
{
	public EditInventoryPopup(EditInventoryPopupViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}


	public async void OnClickedClose(object sender, EventArgs e)
	{
		await CloseAsync();
	}


	public async void OnTappedRecord(object sender, TappedEventArgs e)
	{
		await this.CloseAsync();

		var navParam = new Dictionary<string, object>
			{
				{ "DeliveryId", e.Parameter! },
				{ "IsEditDelivery", false }
			};

        await Shell.Current.GoToAsync(nameof(EditSupplierInventory), true, navParam);
	}
}