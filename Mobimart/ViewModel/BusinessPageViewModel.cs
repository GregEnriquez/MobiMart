using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MobiMart.ViewModel;

public partial class BusinessPageViewModel : BaseViewModel
{
    [ObservableProperty]
    string businessName = "";
    [ObservableProperty]
    string businessAddress = "";
    [ObservableProperty]
    string businessCode = "";

    public BusinessPageViewModel()
    {
        Title = "Business Page";
    }
}
