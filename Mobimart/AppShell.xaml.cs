using MobiMart.View;

namespace MobiMart
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(SignUpPage), typeof(SignUpPage));
            Routing.RegisterRoute(nameof(AddSupplier), typeof(AddSupplier));
            Routing.RegisterRoute(nameof(AddContacts), typeof(AddContacts));
        }
    }
}
