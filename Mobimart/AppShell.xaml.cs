namespace MobiMart
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            this.Loaded += OnShellLoaded;
            Routing.RegisterRoute(nameof(BusinessPage), typeof(BusinessPage));
            Routing.RegisterRoute(nameof(EmployeeTablePage), typeof(EmployeeTablePage));
            Routing.RegisterRoute(nameof(ViewTransaction), typeof(ViewTransaction));
        }
        private async void OnShellLoaded(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//InventoryListPage");
        }
    }
}
