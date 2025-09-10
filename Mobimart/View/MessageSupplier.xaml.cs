namespace MobiMart.View;

public partial class MessageSupplier : ContentPage
{
	public MessageSupplier()
	{
		InitializeComponent();
		BindingContext = new MessageSupplierViewModel();
	}
}