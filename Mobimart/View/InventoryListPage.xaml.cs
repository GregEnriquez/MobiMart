using MobiMart.ViewModel;

namespace MobiMart.View;

public partial class InventoryListPage : ContentPage
{
    public InventoryListPage(InventoryViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
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


    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is InventoryViewModel vm)
        {
            await vm.RefreshInventoryRecords();
        }
    }
}