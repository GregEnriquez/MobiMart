namespace MobiMart;

public partial class BusinessPage : ContentPage
{
	public BusinessPage()
	{
		InitializeComponent();
	}

    private void OnHamburgerClicked(object sender, EventArgs e)
    {
        Shell.Current.FlyoutIsPresented = true;
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnViewClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(EmployeeTablePage), true);
    }
}