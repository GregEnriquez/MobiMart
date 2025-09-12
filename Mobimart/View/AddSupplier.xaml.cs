namespace MobiMart.View;
using MobiMart.ViewModel;

public partial class AddSupplier : ContentPage
{
	public AddSupplier()
	{
		InitializeComponent();
        BindingContext = new AddSupplierViewModel();
	}

    private async void onButtonClicked(object sender, EventArgs args)
    {
        await Shell.Current.GoToAsync(nameof(AddContacts));
    }
}