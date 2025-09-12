namespace MobiMart.View;
using MobiMart.ViewModel;

public partial class AddContacts : ContentPage
{
	public AddContacts()
	{
		InitializeComponent();
		BindingContext = new AddContactsViewModel();
	}
}