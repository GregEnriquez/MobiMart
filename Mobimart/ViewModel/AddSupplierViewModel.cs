using System;
using System.Diagnostics;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobiMart.Model;
using MobiMart.Service;
using MobiMart.View;

namespace MobiMart.ViewModel;

public partial class AddSupplierViewModel : BaseViewModel
{

    UserService userService;
    BusinessService businessService;
    SupplierService supplierService;

    [ObservableProperty]
    string name = "";
    [ObservableProperty]
    string type = "";
    [ObservableProperty]
    string number = "";
    [ObservableProperty]
    string email = "";
    [ObservableProperty]
    string socials = "";
    [ObservableProperty]
    Supplier supplier;
    [ObservableProperty]
    bool editingExisting = false;

    public AddSupplierViewModel(UserService userService, BusinessService businessService, SupplierService supplierService)
    {
        this.userService = userService;
        this.businessService = businessService;
        this.supplierService = supplierService;
    }


    [RelayCommand]
    public async Task SaveChanges()
    {
        if (IsBusy) return;
        IsBusy = true;


        // input validation
        int emptyCount = 0;
        if (Name.Equals("")) emptyCount += 1;
        if (Type.Equals("")) emptyCount += 1;
        if (
            Number.Equals("") &&
            Email.Equals("") &&
            Socials.Equals("")
        ) emptyCount += 1;

        if (emptyCount > 0)
        {
            await Toast.Make("Make sure to fill out all the details", ToastDuration.Short, 14).Show();
            IsBusy = false;
            return;
        }


        var userInstance = await userService.GetUserInstanceAsync();
        var user = await userService.GetUserAsync(userInstance.UserId);

        if (Supplier is null)
        {

            Supplier = new()
            {
                BusinessId = user.BusinessId,
                Type = Type,
                Name = Name,
                Email = Email,  
                Socials = Socials,
                Number = Number
            };
            await supplierService.AddSupplierAsync(Supplier);
            await Toast.Make("Supplier Added Succesfully", ToastDuration.Short, 14).Show();
        }
        else
        {
            Supplier.Name = Name;
            Supplier.Type = Type;
            Supplier.Number = Number;
            Supplier.Email = Email;
            Supplier.Socials = Socials;
            await supplierService.UpdateSupplierAsync(Supplier);
            await Toast.Make("Supplier Updated Succesfully", ToastDuration.Short, 14).Show();
        }

        Name = "";
        Type = "";
        Number = "";
        Email = "";
        Socials = "";
        Supplier = null;
        try
        {
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.StackTrace);
        }

        IsBusy = false;
    }


    public void UpdateInfo()
    {
        Name = Supplier.Name;
        Type = Supplier.Type;
        Number = Supplier.Number;
        Email = Supplier.Email;
        Socials = Supplier.Socials;
        EditingExisting = true;
    }


    public void OnAppearing()
    {
        if (Supplier is not null) return;
        Name = "";
        Type = "";
        Number = "";
        Email = "";
        Socials = "";
        Supplier = null;
    }
}