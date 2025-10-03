using System;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using MobiMart.Model;
using MobiMart.Service;

namespace MobiMart.ViewModel;

public partial class SalesHistoryViewModel : BaseViewModel
{

    [ObservableProperty]
    List<SalesRecord> salesRecords;
    [ObservableProperty]
    DateTime currentDate;


    SalesService salesService;

    public SalesHistoryViewModel(SalesService salesService)
    {
        this.salesService = salesService;
        currentDate = DateTime.Today;
    }


    public async Task RefreshRecords()
    {
        Debug.WriteLine(CurrentDate);
        SalesRecords = await salesService.GetSalesRecordsAsync(CurrentDate);
    }

}
