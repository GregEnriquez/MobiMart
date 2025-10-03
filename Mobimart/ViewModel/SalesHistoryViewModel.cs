using System;
using MobiMart.Service;

namespace MobiMart.ViewModel;

public partial class SalesHistoryViewModel : BaseViewModel
{

    SalesService salesService;

    public SalesHistoryViewModel(SalesService salesService)
    {
        this.salesService = salesService;
    }

}
