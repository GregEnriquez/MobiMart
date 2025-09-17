namespace MobiMart.View;
using MobiMart.Model;
using MobiMart.ViewModel;

[QueryProperty(nameof(Supplier), "Supplier")]
public partial class SupplierInformation : ContentPage
{
    private Supplier? supplierInfo;
    public Supplier? Supplier
    {
        get => supplierInfo;
        set
        {
            supplierInfo = value;
            if (BindingContext is SupplierInfoViewModel vm)
            {
                vm.Supplier = value!;
            }
            // contactsOfSupplier();
        }
    }
    public SupplierInformation(SupplierInfoViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private async void messageSuppClick(object sender, EventArgs args)
    {
        await Shell.Current.GoToAsync(nameof(MessageSupplier));
    }

    private async void suppInventoryClick(object sender, EventArgs args)
    {
        await Shell.Current.GoToAsync(nameof(SupplierInventory));
    }
    private void contactsOfSupplier()
    {
        if (supplierInfo == null) return;

        // var allContacts = new SupplierInfoViewModel().contacts;

        // var filtered = allContacts.Where(c => c.supplierId == supplierInfo.Id).ToList();

        // ContactsSection.ItemsSource = filtered;
    }
}