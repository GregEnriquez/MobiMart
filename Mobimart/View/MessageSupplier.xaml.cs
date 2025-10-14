using MobiMart.ViewModel;

namespace MobiMart.View;

public partial class MessageSupplier : ContentPage
{
	public MessageSupplier(MessageSupplierViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}