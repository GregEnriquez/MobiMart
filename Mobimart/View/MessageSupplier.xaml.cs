using System.Threading.Tasks;
using MobiMart.Model;
using MobiMart.ViewModel;

namespace MobiMart.View;

[QueryProperty(nameof(Supplier), "Supplier")]
public partial class MessageSupplier : ContentPage
{

	private Supplier? supplierInfo;
    public Supplier? Supplier
    {
        get => supplierInfo;
        set
        {
            supplierInfo = value;
            if (BindingContext is MessageSupplierViewModel vm)
            {
                vm.Supplier = value!;
            }
        }
    }

	public MessageSupplier(MessageSupplierViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}


	private async void OnItemSelected(object sender, EventArgs e)
	{
		var picker = (Picker)sender;

		if (BindingContext is MessageSupplierViewModel vm)
		{
			if (picker.BindingContext is MessageRequest request)
			{
				if (picker.SelectedIndex < 0) return;
				await vm.OnItemSelected(picker, request);
			}
		}
	}


	protected override async void OnAppearing()
	{
		base.OnAppearing();

		if (BindingContext is MessageSupplierViewModel vm)
		{
			await vm.OnAppearing();
		}
	}
}