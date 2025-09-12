namespace MobiMart.View;
using MobiMart.ViewModel;

public partial class AddSupplierItem : ContentPage
{
	public AddSupplierItem()
	{
		InitializeComponent();
		BindingContext = new AddSupplierItemViewModel();
	}
}