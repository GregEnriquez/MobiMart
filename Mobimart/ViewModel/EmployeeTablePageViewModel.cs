using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobiMart.Model;
using MobiMart.Service;

namespace MobiMart.ViewModel;

public partial class EmployeeTablePageViewModel : BaseViewModel
{
    UserService userService;
    BusinessService businessService;

    [ObservableProperty]
    List<User> employees;
    [ObservableProperty]
    bool showActions = false;

    public EmployeeTablePageViewModel(UserService userService, BusinessService businessService)
    {
        this.userService = userService;
        this.businessService = businessService;
    }


    public async Task RefreshEmployees()
    {
        var userInstance = await userService.GetUserInstanceAsync();
        var user = await userService.GetUserAsync(userInstance.UserId);
        // var business = await businessService.GetBusinessAsync(user.BusinessRefId);
        Employees = await userService.GetEmployees(user.BusinessId);
        Employees.Remove(user);
    }

    [RelayCommand]
    public async Task Edit()
    {
        ShowActions = !ShowActions;

    }


    [RelayCommand]
    public async Task ConfirmAccept(Guid userId)
    {
        if (IsBusy) return;
        IsBusy = true;

        var user = await userService.GetUserAsync(userId);

        bool confirm = await Shell.Current.DisplayAlert(
            "Confirm Join",
            $"Are you sure you want {user.FirstName} to join your business?",
            "Yes", "No"
        );

        if (confirm)
        {
            user.EmployeeType = "employee";
            await userService.UpdateUserAsync(user);
            await RefreshEmployees();
        }
        else
        {

        }
        IsBusy = false;
    }


    [RelayCommand]
    public async Task DeleteEmployee(Guid userId)
    {
        if (IsBusy) return;
        IsBusy = true;

        var user = await userService.GetUserAsync(userId);

        bool confirm = await Shell.Current.DisplayAlert(
            "Confirm Join",
            $"Are you sure you want to remove {user.FirstName} from your business?",
            "Yes", "No"
        );

        if (confirm)
        {
            user.EmployeeType = "";
            user.BusinessId = Guid.Empty;
            await userService.UpdateUserAsync(user);
            await RefreshEmployees();
        }
        else
        {

        }

        IsBusy = false;
    }
}
