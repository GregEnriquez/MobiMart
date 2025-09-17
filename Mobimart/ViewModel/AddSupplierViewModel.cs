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
    string name;
    [ObservableProperty]
    string type;
    [ObservableProperty]
    string number;
    [ObservableProperty]
    string email;
    [ObservableProperty]
    string socials;
    [ObservableProperty]
    Supplier supplier;

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

        var userInstance = await userService.GetUserInstanceAsync();
        var user = await userService.GetUserAsync(userInstance.UserId);

        if (Supplier is null)
        {

            Supplier = new()
            {
                BusinessId = user.BusinessRefId,
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
            await Shell.Current.GoToAsync("//SupplierList");
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
    }
}


// IMPLEMENT DELETE SUPPLIER
/*
    If supplier is null, its adding a new supplier so change the button's
    text to "Add Supplier"
    If it's not null, then its editing an existing supplier so keep the
    text to "Save Changes" but make a delete button visible that will 
    delete the supplier chosen. Maybe the delete should come some time
    after (its not in the use case, so i wont fucking bother adding that feature)
*/