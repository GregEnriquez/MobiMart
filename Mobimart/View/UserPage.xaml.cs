using MobiMart.ViewModel;

namespace MobiMart.View;

public partial class UserPage : ContentPage
{
	public UserPage(UserPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}