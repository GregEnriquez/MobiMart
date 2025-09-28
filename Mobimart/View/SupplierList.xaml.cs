using System.Diagnostics;
using MobiMart.Model;
using MobiMart.ViewModel;

namespace MobiMart.View;


[QueryProperty(nameof(IsFromInventory), "IsFromInventory")]
public partial class SupplierList : ContentPage
{

    private bool isFromInventory = false;
    public bool IsFromInventory
    {
        get => isFromInventory;
        set
        {
            isFromInventory = value;
            if (BindingContext is SupplierListViewModel vm)
            {
                vm.IsFromInventory = value;
            }
        }
    }

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


    protected override async void OnAppearing()
    {
        base.OnAppearing();

        Debug.WriteLine(IsFromInventory);

        if (BindingContext is SupplierListViewModel vm)
        {
            await vm.RefreshSuppliers();
        }
    }
}