using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobiMart.Model;
using System.Collections.ObjectModel;

namespace MobiMart.ViewModel;

public partial class MessageSupplierViewModel : BaseViewModel
{
    [ObservableProperty]
    private ObservableCollection<MessageRequest> requestedItems = new();
    [ObservableProperty]
    string message;

    public MessageSupplierViewModel()
    {
        requestedItems.Add(new MessageRequest());
    }

    [RelayCommand]
    private async Task AddItem()
    {
        RequestedItems.Add(new MessageRequest());
    }


    [RelayCommand]
    private async Task ComposeMessage()
    {
        if (!Sms.Default.IsComposeSupported) return;

        string[] recipients = new[] { "09701588853" };
        string text = "Hello, I'm interested in buying your vase.";

        var message = new SmsMessage(text, recipients);

        await Sms.Default.ComposeAsync(message);
    }
}
