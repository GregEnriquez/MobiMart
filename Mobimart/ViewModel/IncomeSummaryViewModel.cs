using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MobiMart.ViewModel
{
    public partial class IncomeSummaryViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string todaysIncome;

        public ObservableCollection<ItemSold> ItemsSold { get; set; }

        public IncomeSummaryViewModel()
        {
            Title = "Income Summary";
            TodaysIncome = "0";

            ItemsSold = new ObservableCollection<ItemSold>
            {
                new ItemSold { ItemName = "Apple", Amount = 10, Total = 200, Date = DateTime.Today, ItemType = "Fruit" },
                new ItemSold { ItemName = "Banana", Amount = 5, Total = 100, Date = DateTime.Today, ItemType = "Fruit" },
                new ItemSold { ItemName = "Milk", Amount = 3, Total = 150, Date = DateTime.Today, ItemType = "Dairy" }
            };
        }
    }
}