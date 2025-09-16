using MobiMart.ViewModel;

namespace MobiMart.View;

public partial class InventoryListPage : ContentPage
{
	public InventoryListPage()
	{
		InitializeComponent();
        BindingContext = new InventoryViewModel();
    }

    // private void OnHamburgerClicked(object sender, EventArgs e)
    // {
    //     Shell.Current.FlyoutIsPresented = true;
    // }

    
    // private void PopupClicked(object sender, EventArgs e)
    // {
    //     ItemPopup.SetItemInfo("Milk", "5", "50", "Dairy", "Fresh cow�s milk");
    //     ItemPopup.IsVisible = true;
    // }
}