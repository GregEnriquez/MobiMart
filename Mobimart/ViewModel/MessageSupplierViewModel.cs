using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobiMart.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobiMart.ViewModel
{
    internal class MessageSupplierViewModel
    {
    }
}

public partial class MessageSupplierViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<MessageRequest> requestedItems = new();

    public MessageSupplierViewModel()
    {
        requestedItems.Add(new MessageRequest());
    }

    [RelayCommand]
    private void AddItem()
    {
        requestedItems.Add(new MessageRequest());
    }
}
