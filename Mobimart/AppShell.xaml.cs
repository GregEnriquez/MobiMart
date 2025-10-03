using System.ComponentModel;
using System.Diagnostics;
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
            Routing.RegisterRoute(nameof(AddSupplierItem), typeof(AddSupplierItem));
            Routing.RegisterRoute(nameof(ViewTransaction), typeof(ViewTransaction));

            Routing.RegisterRoute(nameof(SupplierList), typeof(SupplierList));
            Routing.RegisterRoute("SupplierListWrapper", typeof(SupplierList));
            this.Navigating += async (s, e) =>
            {
                if (e.Target.Location.OriginalString.Contains("SupplierListWrapper"))
                {
                    // redirect to supplier list with parameter
                    e.Cancel();
                    try
                    {
                        await Shell.Current.GoToAsync($"{nameof(SupplierList)}?IsFromInventory=false");
                    }
                    catch (Exception err)
                    {
                        Debug.WriteLine(err);
                    }
                }
            };
        }

        private void OnShellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (BindingContext is FlyoutMenuViewModel vm)
            {
                vm.UpdateInfo();
            }
        }
    }
}
