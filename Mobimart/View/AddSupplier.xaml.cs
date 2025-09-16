using MobiMart.ViewModel;

namespace MobiMart.View;
using MobiMart.ViewModel;

public partial class AddSupplier : ContentPage
{
<<<<<<< HEAD
    public AddSupplier(AddSupplierViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
=======
	public AddSupplier()
	{
		InitializeComponent();
        BindingContext = new AddSupplierViewModel();
>>>>>>> 171744497a5565ccb09f38e86824fd77d04058f0
	}

    private async void onButtonClicked(object sender, EventArgs args)
    {
        await Shell.Current.GoToAsync(nameof(AddContacts));
    }
}