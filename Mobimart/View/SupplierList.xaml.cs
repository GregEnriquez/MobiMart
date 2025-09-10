using MobiMart.Model;
using MobiMart.ViewModel;

namespace MobiMart.View;

public partial class SupplierList : ContentPage
{
	public SupplierList()
	{
		InitializeComponent();
        //Suppliers.ItemsSource = getSuppliers();
        BindingContext = new SupplierListViewModel();
    }

    /*private List<Supplier> getSuppliers()
    {
        return new List<Supplier>
        {
            new Supplier
            {
                Id=1,
                BusinessId=1,
                Type="Consignment",
                Name="Marlboro",
                Email = "email@sample.com",
                Number="09123456789",
                SocialsId=1,
                LastModified="2025/09/09"
            },

            new Supplier
            {
                Id=2,
                BusinessId=2,
                Type="Wholesale",
                Name="Zesto",
                Email = "email2@sample.com",
                Number="09123456781",
                SocialsId=1,
                LastModified="2025/09/09"
            }
         };
    }*/

    private async void onButtonClicked(object sender, EventArgs args)
    {
        await Shell.Current.GoToAsync(nameof(AddSupplier));
    }

    private async void onSupplierTap(object sender, EventArgs args)
    {
        await Shell.Current.GoToAsync(nameof(SupplierInformation));
    }
}