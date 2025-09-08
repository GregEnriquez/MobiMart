using MobiMart.ViewModel;

namespace MobiMart.View;

public partial class BusinessPage : ContentPage
{
	public BusinessPage(BusinessPageViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;
	}
}