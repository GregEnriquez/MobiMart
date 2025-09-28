namespace MobiMart.View;

using System.Diagnostics;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using MobiMart.Model;
using MobiMart.ViewModel;
using ZXing.Net.Maui;

[QueryProperty(nameof(Supplier), "Supplier")]
[QueryProperty(nameof(IsFromInventory), "IsFromInventory")]
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

	public AddSupplierItem(AddSupplierItemViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;

		barcodeReader.Options = new BarcodeReaderOptions
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

			barcodeReader.IsTorchOn = false;
			await vm.HideScanner();
		}
	}


	private void onPressedTorchButton(object sender, EventArgs e)
	{
		barcodeReader.IsTorchOn = !barcodeReader.IsTorchOn;
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