using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MobiMart.Model
{
    public class MessageRequest : INotifyPropertyChanged
    {
        private string itemName = "";
        public string ItemName { get => itemName; set { itemName = value; OnPropertyChanged(); } }
        private int quantity;
        public int Quantity { get => quantity; set { quantity = value; OnPropertyChanged(); } }
        private Item item;
        public Item Item { get => item; set { item = value; OnPropertyChanged(); }}


        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "") =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
