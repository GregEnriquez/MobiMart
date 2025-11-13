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
                vm.UpdateSelectedRecords();
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

    private async void OnCheckboxClicked(object sender, EventArgs e)
    {
        CheckBox cb = (CheckBox)sender;
        SalesItem item = (SalesItem)cb.BindingContext;

        cb.IsChecked = !cb.IsChecked;

        
        if (BindingContext is ViewTransactionViewModel vm)
        {
            vm.OnCheckboxClicked(cb, item);
        }
    }
}