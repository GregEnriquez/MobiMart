using MobiMart.ViewModel;

namespace MobiMart.View;

using Android.Systems;
using MobiMart.Model;
using MobiMart.ViewModel;

[QueryProperty(nameof(Supplier), "Supplier")]
public partial class AddSupplier : ContentPage
{
    private Supplier supplier;
    public Supplier Supplier
    {
        get => supplier;
        set
        {
            supplier = value;
            if (BindingContext is AddSupplierViewModel vm)
            {
                vm.Supplier = value;
                vm.UpdateInfo();
            }
        }
    }

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