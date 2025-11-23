using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobiMart.Model;
using MobiMart.Service;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MobiMart.ViewModel;

public partial class MessageSupplierViewModel : BaseViewModel
{
    public ObservableCollection<MessageRequest> RequestedItems { get; set; } = new ObservableCollection<MessageRequest>();
    [ObservableProperty]
    string message;
    [ObservableProperty]
    List<Item> allItems;
    [ObservableProperty]
    Supplier supplier;

    private string businessName;
    private string businessAddress;
    private string requesteeName;

    InventoryService inventoryService;
    UserService userService;
    BusinessService businessService;

    public MessageSupplierViewModel(InventoryService inventoryService, UserService userService, BusinessService businessService)
    {
        this.inventoryService = inventoryService;
        this.userService = userService;
        this.businessService = businessService;
        RequestedItems = [new MessageRequest()];
        RequestedItems[^1].PropertyChanged += OnMessageRequestPropertyChanged;
    }

    [RelayCommand]
    private async Task AddItem()
    {
        if (RequestedItems.Count == AllItems.Count)
        {
            await Toast.Make("Can't add any more items", ToastDuration.Short, 14).Show();
            return;
        }
        RequestedItems.Add(new MessageRequest());
        RequestedItems[^1].PropertyChanged += OnMessageRequestPropertyChanged;
    }


    [RelayCommand]
    public void DeleteRequest(MessageRequest request)
    {
        if (RequestedItems.Count <= 1)
        {
            RequestedItems = [];
            return;
        }
        RequestedItems.Remove(request);
        UpdateMessage();
    }


    [RelayCommand]
    private async Task ComposeMessage()
    {
        UpdateMessage();
        if (!Sms.Default.IsComposeSupported) return;

        string[] recipients = new[] { Supplier.Number };

        var message = new SmsMessage(Message, recipients);

        await Sms.Default.ComposeAsync(message);
    }


    public async Task OnItemSelected(Picker picker, MessageRequest selectedRequest)
    {
        if (RequestedItems is null || RequestedItems.Count <= 0) return;

        foreach (var req in RequestedItems)
        {
            if (req.Item is null) continue;
            if (Object.ReferenceEquals(req, selectedRequest)) continue;

            if (req.Item.Name.Equals(selectedRequest.Item.Name))
            {
                picker.SelectedIndex = -1;
                selectedRequest.Item = null;
                selectedRequest.ItemName = "";
                await Toast.Make("Item already added\nYou can change the quantity if you want to add more of this item", ToastDuration.Short, 14).Show();
                return;
            }
        }

        UpdateMessage();
    }


    public void UpdateMessage()
    {
        if (businessName is null) return;
        
        Message = $"""
        From: {businessName}, {requesteeName}

        Requesting to deliver these items on {businessAddress}:


        """;

        foreach (var reqItem in RequestedItems)
        {
            if (reqItem.Item is null) continue;
            Message += $"{reqItem.Item.Name} - {reqItem.Quantity}pcs\n";
        }
    }


    private void OnMessageRequestPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(MessageRequest.Quantity))
        {
            UpdateMessage();
        }
    }


    public async Task OnAppearing()
    {
        var businessId = -1;
        if (Shell.Current.BindingContext is FlyoutMenuViewModel vm)
        {
            businessId = vm.BusinessId;
        }
        var business = await businessService.GetBusinessAsync(businessId);
        businessName = business.Name;
        businessAddress = business.Address;

        var userInstance = await userService.GetUserInstanceAsync();
        var user = await userService.GetUserAsync(userInstance.UserId);
        requesteeName = user.FirstName;

        AllItems = await inventoryService.GetAllItemsAsync();
    }
}
