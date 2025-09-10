using MobiMart.ViewModel;

namespace MobiMart.View;

public partial class TransactionPage : ContentPage
{
	public TransactionPage()
	{
		InitializeComponent();
        BindingContext = new TransactionViewModel();
    }

    private void OnHamburgerClicked(object sender, EventArgs e)
    {
        Shell.Current.FlyoutIsPresented = true;
    }
}