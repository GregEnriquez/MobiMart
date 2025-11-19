using System.Diagnostics;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using MobiMart.Model;
using MobiMart.ViewModel;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;

namespace MobiMart.View;

public partial class TransactionPage : ContentPage
{

    private CameraBarcodeReaderView? barcodeReader;


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


    private void OnBarcodeEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        if (e.NewTextValue.Length == 13)
		{
            if (BindingContext is TransactionViewModel vm)
            {
                Entry entry = (Entry)sender;
                vm.PickItem(e.NewTextValue, (Transaction)entry.BindingContext);
                entry.Focus();
			}
		}
    }
    
    private void OnPressedTorchButton(object sender, EventArgs e)
	{
		barcodeReader!.IsTorchOn = !barcodeReader.IsTorchOn;
	}

    private void OnPressedCloseButton(object sender, EventArgs e)
    {
        barcodeReader!.IsTorchOn = false;
        if (BindingContext is TransactionViewModel vm)
        {
            vm.HideScanner();
        }
    }


    private void BarcodeReader_BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        var first = e.Results?.FirstOrDefault();
        if (first is null) return;

        if (BindingContext is TransactionViewModel vm)
        {
            bool hasRecord = vm.PickItem(first.Value);
            if (!hasRecord)
            {
                Dispatcher.Dispatch(async () =>
                {
                    await Toast.Make("No item found with that barcode", ToastDuration.Short, 14).Show();
                });
            }

            barcodeReader!.IsTorchOn = false;
            vm.HideScanner();
        }
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();
        barcodeReader = new CameraBarcodeReaderView
        {
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
            IsDetecting = true,
            HeightRequest = 250,
            WidthRequest = 350
        };
        barcodeReader.BarcodesDetected += BarcodeReader_BarcodesDetected;

        ((VerticalStackLayout)ScannerOverlay.Children.First()).Children.Insert(0, barcodeReader);

        if (BindingContext is TransactionViewModel vm)
        {
            await vm.OnAppearing();
        }
    }


    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        if (barcodeReader is null) return;

        barcodeReader.IsDetecting = false;
        barcodeReader.BarcodesDetected -= BarcodeReader_BarcodesDetected;
        ((VerticalStackLayout)ScannerOverlay.Children.First()).Children.Remove(barcodeReader);
        barcodeReader = null;
    }
}