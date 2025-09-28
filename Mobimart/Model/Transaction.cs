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

        private float price;
        public float Price
        {
            get => price;
            set { price = value; OnPropertyChanged(); OnQuantityOrPriceChanged?.Invoke(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public Action OnQuantityOrPriceChanged { get; set; }
    }
}
