namespace MobiMart.View;

public partial class IncomeSummary : ContentPage
{
	public IncomeSummary()
	{
		InitializeComponent();
	}

    private void OnHamburgerClicked(object sender, EventArgs e)
    {
        Shell.Current.FlyoutIsPresented = true;
    }
}