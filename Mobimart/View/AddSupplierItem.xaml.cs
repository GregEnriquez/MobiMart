namespace MobiMart.View;

using System.Diagnostics;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using MobiMart.Model;
using MobiMart.ViewModel;
using ZXing.Net.Maui;

[QueryProperty(nameof(Supplier), "Supplier")]
[QueryProperty(nameof(IsFromInventory), "IsFromInventory")]
[QueryProperty(nameof(IsFromScanBarcode), "IsFromScanBarcode")]
public partial class AddSupplierItem : ContentPage
{
	private Supplier? supplier;
	public Supplier? Supplier
	{
		get => supplier;
		set
		{
			supplier = value;
			if (BindingContext is AddSupplierItemViewModel vm)
			{
				vm.Supplier = value;
			}
		}
	}

	private bool isFromInventory = false;
	public bool IsFromInventory
	{
		get => isFromInventory;
		set
		{
			isFromInventory = value;
			if (BindingContext is AddSupplierItemViewModel vm)
			{
				vm.IsFromInventory = value;
			}
		}
	}
	private bool isFromScanBarcode = false;
	public bool IsFromScanBarcode
	{
		get => isFromScanBarcode;
		set
		{
			isFromScanBarcode = value;
			if (BindingContext is AddSupplierItemViewModel vm)
			{
				vm.IsFromScanBarcode = value;
				if (value) vm.IsScannerVisible = true;
			}
		}
	}

	public AddSupplierItem(AddSupplierItemViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;

		invBarcodeReader.Options = new BarcodeReaderOptions
		{
			Formats = BarcodeFormat.Ean13,
			TryHarder = true
		};
	}


	private async void barcodeReader_BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
	{
		var first = e.Results?.FirstOrDefault();
		if (first is null) return;

		if (BindingContext is AddSupplierItemViewModel vm)
		{
			bool hasRecord = await vm.FillItemDetails(first.Value);
			string message = hasRecord ? "Item Deatils Filled" : $"NO record found for barcode: {first.Value}";

			Dispatcher.Dispatch(async () =>
			{
				await Toast.Make(message, ToastDuration.Short, 14).Show();
			});

			invBarcodeReader.IsTorchOn = false;
			vm.HideScanner();
		}
	}


	private void onPressedTorchButton(object sender, EventArgs e)
	{
		invBarcodeReader.IsTorchOn = !invBarcodeReader.IsTorchOn;
	}


	private void onPressedCloseButton(object sender, EventArgs e)
    {
        invBarcodeReader.IsTorchOn = false;
		if (BindingContext is AddSupplierItemViewModel vm)
        {
            vm.HideScanner();
        }
    }


	private async void OnBarcodeEntryTextChanged(object sender, TextChangedEventArgs e)
	{
		if (e.NewTextValue.Length == 13)
		{
			if (BindingContext is AddSupplierItemViewModel vm)
			{
				await vm.FillItemDetails(e.NewTextValue);
			}
		}
		else if (e.NewTextValue.Length == 12 && e.OldTextValue.Length == 13)
		{
			if (BindingContext is AddSupplierItemViewModel vm)
			{
				await vm.ClearItemDetails();
			}
		}
	}
}