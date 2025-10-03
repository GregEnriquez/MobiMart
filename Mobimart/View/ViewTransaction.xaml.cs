using MobiMart.Model;
using MobiMart.ViewModel;

namespace MobiMart.View;

[QueryProperty(nameof(Record), "SalesRecord")]
public partial class ViewTransaction : ContentPage
{
    public ViewTransaction(ViewTransactionViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
	}

    private SalesRecord _record;
    public SalesRecord Record
    {
        get => _record;
        set
        {
            _record = value;
            if (BindingContext is ViewTransactionViewModel vm)
            {
                vm.Record = value;
            }
        }
    }

    // private void LoadTransaction()
    // {
    //     if (_record == null) return;
    // }

    // private void OnHamburgerClicked(object sender, EventArgs e)
    // {
    //     Shell.Current.FlyoutIsPresented = true;
    // }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}