using CommunityToolkit.Maui.Extensions;
using Microsoft.Maui.Controls;
using MobiMart.Model;
using MobiMart.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MobiMart.ViewModel
{
    class EditInventoryViewModel
    {
        public Inventory Item { get; set; }
        public ICommand SaveCommand { get; set; }

        // public string ItemName
        // {
        //     get => Item.ItemName;
        //     set => Item.ItemName = value;
        // }
        // public int TotalAmount
        // {
        //     get => Item.TotalAmount;
        //     set => Item.TotalAmount = value;
        // }
        // public float RetailPrice
        // {
        //     get => Item.RetailPrice;
        //     set => Item.RetailPrice = value;
        // }
        // public string ItemType
        // {
        //     get => Item.ItemType;
        //     set => Item.ItemType = value;
        // }

        public EditInventoryViewModel(Inventory item)
        {
            Item = item;
            // SaveCommand = new Command(async () => await Save());
        }

        // private async Task Save()
        // {
        //     var popup = new SaveInventoryPopup($"{Item.ItemName} updated!");
        //     var result = await Application.Current.MainPage.ShowPopupAsync(popup);
        //     await Application.Current.MainPage.Navigation.PopAsync();
        // }
    }
}
