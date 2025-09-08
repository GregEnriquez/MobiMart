namespace MobiMart.View;

public partial class TransactionPage : ContentPage
{
	public TransactionPage()
	{
		InitializeComponent();
	}

    private void OnHamburgerClicked(object sender, EventArgs e)
    {
        Shell.Current.FlyoutIsPresented = true;
    }
}