using MobiMart.View;

namespace MobiMart
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(SignUpPage), typeof(SignUpPage));
            // this.Loaded += OnShellLoaded!;
            // Routing.RegisterRoute(nameof(UserPage), typeof(UserPage));
            // Routing.RegisterRoute(nameof(BusinessPage), typeof(BusinessPage));
            // Routing.RegisterRoute(nameof(EmployeeTablePage), typeof(EmployeeTablePage));
            // Routing.RegisterRoute(nameof(ViewTransaction), typeof(ViewTransaction));
            // Routing.RegisterRoute(nameof(SupplierInformation), typeof(SupplierInformation));
            // Routing.RegisterRoute(nameof(AddSupplier), typeof(AddSupplier));
            // Routing.RegisterRoute(nameof(AddContacts), typeof(AddContacts));
            // Routing.RegisterRoute(nameof(MessageSupplier), typeof(MessageSupplier));
            // Routing.RegisterRoute(nameof(SupplierInventory), typeof(SupplierInventory));
            // Routing.RegisterRoute(nameof(EditSupplierInventory), typeof(EditSupplierInventory));
            Routing.RegisterRoute(nameof(AddSupplier), typeof(AddSupplier));
            Routing.RegisterRoute(nameof(AddContacts), typeof(AddContacts));
            Routing.RegisterRoute(nameof(SupplierInformation), typeof(SupplierInformation));
            Routing.RegisterRoute(nameof(MessageSupplier), typeof(MessageSupplier));
            Routing.RegisterRoute(nameof(SupplierInventory), typeof(SupplierInventory));
            Routing.RegisterRoute(nameof(EditSupplierInventory), typeof(EditSupplierInventory));
            Routing.RegisterRoute(nameof(ViewTransaction), typeof(ViewTransaction));
        }
        // private async void OnShellLoaded(object sender, EventArgs e)
        // {
        //     await Shell.Current.GoToAsync("//InventoryListPage");
        // }
    }
}
