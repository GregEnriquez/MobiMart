using System;
using CommunityToolkit.Mvvm.ComponentModel;
using MobiMart.Model;

namespace MobiMart.ViewModel;

public partial class ViewTransactionViewModel : BaseViewModel
{

    [ObservableProperty]
    SalesRecord record;

    public ViewTransactionViewModel()
    {
        
    }
    

}
