using MobiMart.ViewModel;

namespace MobiMart.View;
using MobiMart.ViewModel;

public partial class AddSupplier : ContentPage
{
    public AddSupplier(AddSupplierViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
	}

    private async void onButtonClicked(object sender, EventArgs args)
    {
        await Shell.Current.GoToAsync(nameof(AddContacts));
    }
}