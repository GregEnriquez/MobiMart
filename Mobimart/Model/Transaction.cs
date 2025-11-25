using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MobiMart.Model
{
    public class Transaction : INotifyPropertyChanged
    {
        private string itemName;
        public string ItemName
        {
            get => itemName;
            set { itemName = value; OnPropertyChanged(); }
        }

        private int quantity;
        public int Quantity
        {
            get => quantity;
            set { quantity = value; OnPropertyChanged(); OnQuantityOrPriceChanged?.Invoke(); }
        }

        private decimal price;
        public decimal Price
        {
            get => price;
            set { price = value; OnPropertyChanged(); OnQuantityOrPriceChanged?.Invoke(); }
        }

        private Item _selectedItem;
        public Item SelectedItem
        {
            get => _selectedItem;
            set { _selectedItem = value; OnPropertyChanged(); }
        }
        private bool _isBarcodeEntry = true;
        public bool IsBarcodeEntry
        {
            get => _isBarcodeEntry;
            set { _isBarcodeEntry = value;  OnPropertyChanged(); }
        }

        public string BarcodeId { get; set; } = "";
        public int SelectedIndex { get; set; } = -1;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public Action OnQuantityOrPriceChanged { get; set; }
    }
}
