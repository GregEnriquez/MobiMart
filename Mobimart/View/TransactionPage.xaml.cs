using System.Diagnostics;
using MobiMart.Model;
using MobiMart.ViewModel;

namespace MobiMart.View;

public partial class TransactionPage : ContentPage
{
    public TransactionPage(TransactionViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    // private void OnHamburgerClicked(object sender, EventArgs e)
    // {
    //     Shell.Current.FlyoutIsPresented = true;
    // }


    private async void OnItemNameEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        if (BindingContext is TransactionViewModel vm)
        {
            await vm.OnEntryTextChanged(e.NewTextValue);
        }
    }


    private async void OnSuggestionLabelTapped(object sender, TappedEventArgs e)
    {
        if (BindingContext is TransactionViewModel vm)
        {
            await vm.SelectItem(e.Parameter!.ToString()!);
        }
    }


    private async void OnItemSelected(object sender, EventArgs e)
    {
        var picker = (Picker)sender;

        if (BindingContext is TransactionViewModel vm)
        {
            if (picker.BindingContext is Transaction item)
            {
                if (picker.SelectedIndex < 0) return;
                vm.OnItemSelected(picker, item);
            }
        }
    }


    private void OnPaymentTextChanged(object sender, EventArgs e)
    {
        if (BindingContext is TransactionViewModel vm)
        {
            vm.UpdateChange();
        }
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is TransactionViewModel vm)
        {
            await vm.OnAppearing();
        }
    }
}