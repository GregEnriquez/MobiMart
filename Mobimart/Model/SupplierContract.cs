using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MobiMart.Model;

public class SupplierContract : INotifyPropertyChanged
{
    public int SupplierId { get; set; }
    private string supplierName = "";
    public string SupplierName { get => supplierName; set { supplierName = value; OnPropertyChanged(); } }
    private DateTime returnDate;
    public DateTime ReturnDate { get => returnDate; set { returnDate = value; OnPropertyChanged(); } }
    private bool isDropped = false;
    public bool IsDropped { get => isDropped; set { isDropped = value; OnPropertyChanged(); } }
    private List<ContractItem> items = [];
    public List<ContractItem> Items { get => items; set { items = value; OnPropertyChanged(); } }
    private float amountToPay;
    public float AmountToPay { get => amountToPay; set { amountToPay = value; OnPropertyChanged(); } }
    private ImageSource? imageSource = null;
    public ImageSource? ImageSource { get => imageSource; set { imageSource = value; OnPropertyChanged(); } }
    public byte[] ImageData { get; set; }
    private bool hasProof = false;
    public bool HasProof { get => hasProof; set { hasProof = value; OnPropertyChanged(); } }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}


public record class ContractItem
{
    public string Name { get; set; } = "";
    public int SoldQuantity { get; set; }
    public int ReturnQuantity { get; set; }
}
