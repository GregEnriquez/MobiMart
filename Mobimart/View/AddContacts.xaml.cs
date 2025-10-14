namespace MobiMart.View;
using MobiMart.ViewModel;

public partial class AddContacts : ContentPage
{
	public AddContacts(AddContactsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel; 
	}
}