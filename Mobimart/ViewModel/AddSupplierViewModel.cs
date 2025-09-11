using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobiMart.Model;
using MobiMart.Service;

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

    public AddSupplierViewModel(UserService userService, BusinessService businessService, SupplierService supplierService)
    {
        this.userService = userService;
        this.businessService = businessService;
        this.supplierService = supplierService;
    }


    [RelayCommand]
    public async Task SaveChanges()
    {
        var userInstance = await userService.GetUserInstanceAsync();
        var user = await userService.GetUserAsync(userInstance.UserId);
        var business = await businessService.GetBusinessAsync(user.BusinessRefId);

        Supplier supplier = new()
        {
            BusinessId = user.BusinessRefId,
            Type = Type,
            Name = Name,
            Email = Email,
            Socials = Socials,
            Number = Number
        };

        await supplierService.AddSupplierAsync(supplier);
    }
}
