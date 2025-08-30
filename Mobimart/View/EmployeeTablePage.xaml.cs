namespace MobiMart.View;

public partial class EmployeeTablePage : ContentPage
{
	public EmployeeTablePage()
	{
		InitializeComponent();
		Routing.RegisterRoute("EmployeeTablePage", typeof(EmployeeTablePage));
	}
}