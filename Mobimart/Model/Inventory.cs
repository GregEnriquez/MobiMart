using SQLite;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace MobiMart.Model;

public class Inventory : INotifyPropertyChanged
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    [ForeignKey(nameof(Business))]
    public int BusinessId { get; set; }
    [ForeignKey(nameof(WholeSaleInventory))]
    public int DeliveryId { get; set; } // refers to wholesale inventory
    [ForeignKey(nameof(Description))]
    public int DescriptionId { get; set; } // DEV NOTE: I don't think this is supposed to be here (I just copied whats on the ERD)
    private int _totalAmount;
    public int TotalAmount
    {
        get => _totalAmount;
        set { _totalAmount = value; OnPropertyChanged(); }
    }

    private string _itemName = "";
    public string ItemName
    {
        get => _itemName;
        set { _itemName = value; OnPropertyChanged(); }
    }
    private float _retailPrice;
    public float RetailPrice
    {
        get => _retailPrice;
        set { _retailPrice = value; OnPropertyChanged(); }
    }

    private string _itemType = "";
    public string ItemType
    {
        get => _itemType;
        set { _itemType = value; OnPropertyChanged(); }
    }
    public required string LastModified { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
