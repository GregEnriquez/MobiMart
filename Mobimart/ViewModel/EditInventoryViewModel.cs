using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MobiMart.Model;
using Microsoft.Maui.Controls;

namespace MobiMart.ViewModel
{
    class EditInventoryViewModel
    {
        public Inventory Item { get; set; }
        public ICommand SaveCommand { get; set; }

        public string ItemName
        {
            get => Item.ItemName;
            set => Item.ItemName = value;
        }
        public int TotalAmount
        {
            get => Item.TotalAmount;
            set => Item.TotalAmount = value;
        }
        public float RetailPrice
        {
            get => Item.RetailPrice;
            set => Item.RetailPrice = value;
        }
        public string ItemType
        {
            get => Item.ItemType;
            set => Item.ItemType = value;
        }

        public EditInventoryViewModel(Inventory item)
        {
            Item = item;
            SaveCommand = new Command(async () => await Save());
        }

        private async Task Save()
        {
            await Application.Current.MainPage.DisplayAlert("Saved", $"{Item.ItemName} updated!", "OK");
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}
