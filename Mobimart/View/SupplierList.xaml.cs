using System.Diagnostics;
using MobiMart.Model;
using MobiMart.ViewModel;

namespace MobiMart.View;

public partial class SupplierList : ContentPage
{
    public SupplierList(SupplierListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private async void onButtonClicked(object sender, EventArgs args)
    {
        await Shell.Current.GoToAsync(nameof(AddSupplier));
    }

    private async void onSupplierTap(object sender, TappedEventArgs args)
    {
        // if (args.Parameter is Supplier supplier)
        // {
        //     var navParameter = new Dictionary<string, object>()
        //     {
        //         {"Supplier", supplier }
        //     };

        //     await Shell.Current.GoToAsync(nameof(SupplierInformation), navParameter);
        // }
    }


    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is SupplierListViewModel vm)
        {
            vm.RefreshSuppliers();
        }
    }
}