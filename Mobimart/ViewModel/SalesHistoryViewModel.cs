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
    [ObservableProperty]
    bool recordsEmpty = false;


    SalesService salesService;

    public SalesHistoryViewModel(SalesService salesService)
    {
        this.salesService = salesService;
        currentDate = DateTime.Today;
    }


    public async Task RefreshRecords()
    {
        if (IsBusy) return;
        IsBusy = true;

        RecordsEmpty = true;
        SalesRecords = await salesService.GetSalesRecordsAsync(CurrentDate);
        if (SalesRecords.Count > 0)
        {
            RecordsEmpty = false;
        }

        IsBusy = false;
    }

}
