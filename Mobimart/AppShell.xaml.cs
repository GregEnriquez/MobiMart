using System.ComponentModel;
using MobiMart.Service;
using MobiMart.View;
using MobiMart.ViewModel;

namespace MobiMart
{
    public partial class AppShell : Shell
    {
        public AppShell(FlyoutMenuViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = viewModel;

            // this.Loaded += OnShellLoaded!;
            this.PropertyChanged += OnShellPropertyChanged;
            // Routing.RegisterRoute(nameof(UserPage), typeof(UserPage));
            // Routing.RegisterRoute(nameof(BusinessPage), typeof(BusinessPage));

            Routing.RegisterRoute(nameof(SignUpPage), typeof(SignUpPage));
            Routing.RegisterRoute(nameof(EmployeeTablePage), typeof(EmployeeTablePage));
            Routing.RegisterRoute(nameof(ViewTransaction), typeof(ViewTransaction));
            Routing.RegisterRoute(nameof(SupplierInformation), typeof(SupplierInformation));
            Routing.RegisterRoute(nameof(AddSupplier), typeof(AddSupplier));
            Routing.RegisterRoute(nameof(AddContacts), typeof(AddContacts));
            Routing.RegisterRoute(nameof(MessageSupplier), typeof(MessageSupplier));
            Routing.RegisterRoute(nameof(SupplierInventory), typeof(SupplierInventory));
            Routing.RegisterRoute(nameof(EditSupplierInventory), typeof(EditSupplierInventory));

            // Routing.RegisterRoute(nameof(SignUpPage), typeof(SignUpPage));
            // Routing.RegisterRoute(nameof(AddSupplier), typeof(AddSupplier));
            // Routing.RegisterRoute(nameof(AddContacts), typeof(AddContacts));
            // Routing.RegisterRoute(nameof(SupplierInformation), typeof(SupplierInformation));
            // Routing.RegisterRoute(nameof(MessageSupplier), typeof(MessageSupplier));
            // Routing.RegisterRoute(nameof(SupplierInventory), typeof(SupplierInventory));
            // Routing.RegisterRoute(nameof(EditSupplierInventory), typeof(EditSupplierInventory));
        }
        // private async void OnShellLoaded(object sender, EventArgs e)
        // {
        //     await Shell.Current.GoToAsync("//InventoryListPage");
        // }
        private void OnShellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (BindingContext is FlyoutMenuViewModel vm)
		{
			vm.UpdateInfo();
		}
        }
    }
}
