using System;
using System.Diagnostics;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobiMart.Model;
using MobiMart.Service;

namespace MobiMart.ViewModel;

public partial class ContractsViewModel : BaseViewModel
{

    [ObservableProperty]
    List<SupplierContract> supplierContracts = [];
    [ObservableProperty]
    bool isShowCaptured = false;
    [ObservableProperty]
    ImageSource? imageSource = null;
    [ObservableProperty]
    bool isLoadingRecords = true;

    private byte[]? imageData = null;
    SupplierContract? contractUsingCam;

    SupplierService supplierService;
    InventoryService inventoryService;

    public ContractsViewModel(SupplierService supplierService, InventoryService inventoryService)
    {
        this.supplierService = supplierService;
        this.inventoryService = inventoryService;
    }


    public async Task OnAppearing()
    {
        SupplierContracts = [];
        IsLoadingRecords = true;
        var suppliers = await supplierService.GetAllSuppliersAsync();

        // get all the suppliercontracts within the last 7 days
        for (int i = 0; i < suppliers.Count; i++)
        {
            var supplier = suppliers[i];
            if (!supplier.Type.ToLower().Equals("consignment")) continue;

            for (int j = 0; j <= 7; j++)
            {
                DateTime date = DateTime.Now.AddDays(j);
                var contract = await inventoryService.GetSupplierContractAsync(supplier, date);
                if (contract is null) continue;
                SupplierContracts.Add(contract);
            }
        }

        // get all the completed/returned contract
        var returnedSupplierContracts = await supplierService.GetReturnedSupplierContractsAsync();
        foreach (var contract in returnedSupplierContracts)
        {
            SupplierContracts.Add(contract);
        }


        SupplierContracts = [.. SupplierContracts.AsEnumerable()];
        IsLoadingRecords = false;
    }


    [RelayCommand]
    public async Task ContractTapped(SupplierContract supplierContract)
    {
        supplierContract.IsDropped = !supplierContract.IsDropped;
    }


    [RelayCommand]
    public async Task ShowCamera(SupplierContract supplierContract)
    {
        this.contractUsingCam = supplierContract;

        try
        {
            FileResult? photo = await MediaPicker.CapturePhotoAsync();

            if (photo != null)
            {
                using var stream = await photo.OpenReadAsync();
                using var ms = new MemoryStream();
                await stream.CopyToAsync(ms);
                imageData = ms.ToArray();

                ImageSource = ImageSource.FromStream(() => new MemoryStream(ms.ToArray()));
            }
        }
        catch (FeatureNotSupportedException)
        {
            await Shell.Current.DisplayAlert("Error", "Camera not supported on this device.", "OK");
        }
        catch (PermissionException e)
        {
            await Shell.Current.DisplayAlert("Error", "Camera permission denied.", "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }

        IsShowCaptured = true;
    }


    [RelayCommand]
    public void HideCamera()
    {
        this.contractUsingCam = null;
        IsShowCaptured = false;
        imageData = null;
        ImageSource = null;
    }


    [RelayCommand]
    public async Task SaveProof()
    {
        contractUsingCam!.ImageSource = ImageSource!;
        contractUsingCam!.ImageData = imageData!;
        contractUsingCam.HasProof = true;

        IsShowCaptured = false;
    }


    [RelayCommand]
    public async Task CompleteContract(SupplierContract supplierContract)
    {
        if (IsBusy) return;
        IsBusy = true;
        // input validation
        if (!supplierContract.HasProof)
        {
            await Toast.Make("Be sure to take a picture as proof of delivery", ToastDuration.Long, 14).Show();
            IsBusy = false;
            return;
        }

        var businessId = Guid.Empty;
        if (Shell.Current.BindingContext is FlyoutMenuViewModel vm)
        {
            businessId = vm.BusinessId;
        }
        // save completed contract to database
        CompletedContract complete = new()
        {
            BusinessId = businessId,
            SupplierName = supplierContract.SupplierName,
            ReturnDate = supplierContract.ReturnDate.ToUniversalTime(),
            DateReturned = DateTimeOffset.UtcNow,
            AmountToPay = supplierContract.AmountToPay,
            ProofImageData = supplierContract.ImageData
        };
        await supplierService.AddCompletedContractAsync(complete);
        // as well the items returned
        for (int i = 0; i < supplierContract.Items.Count; i++)
        {
            var item = supplierContract.Items[i];
            var completeItem = new CompletedContractItem()
            {
                ContractId = complete.Id,
                Name = item.Name,
                SoldQuantity = item.SoldQuantity,
                ReturnQuantity = item.ReturnQuantity
            };
            await supplierService.AddCompletedContractItemAsync(completeItem);
        }

        // remove items from inventory
        var supplier = await supplierService.GetSupplierAsync(supplierContract.SupplierId);
        await inventoryService.RemoveCongsignmentInventoryViaReturnDate(supplier, supplierContract.ReturnDate);

        // finally, remove supplier contract record
        SupplierContracts.Remove(supplierContract);
        SupplierContracts = [.. SupplierContracts.AsEnumerable()];

        IsBusy = false;
    }


    // [RelayCommand]
    // public async Task CameraMediaCaptured(Stream capturedStream)
    // {
    //     var memoryStream = new MemoryStream();
    //     await capturedStream.CopyToAsync(memoryStream);
    //     byte[] imageData = memoryStream.ToArray();

    //     try
    //     {
    //         ImageSource = ImageSource.FromStream(() => new MemoryStream(imageData));
    //     }
    //     catch (Exception e)
    //     {
            
    //     }
    // }
}
