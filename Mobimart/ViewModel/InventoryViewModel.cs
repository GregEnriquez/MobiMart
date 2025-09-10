using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MobiMart.Model;
using Microsoft.Maui.Controls;
using MobiMart.View;

namespace MobiMart.ViewModel
{
    class InventoryViewModel : BindableObject
    {
        public ObservableCollection<Inventory> InventoryItems { get; set; }

        private ObservableCollection<Inventory> _allItems; // original list

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                FilterItems(_searchText);
            }
        }


        public ICommand AddItemCommand { get; set; }
        public ICommand EditCommand { get; set; }

        public InventoryViewModel()
        {
            InventoryItems = new ObservableCollection<Inventory>();
            _allItems = new ObservableCollection<Inventory>();

            //_allItems.Add(newItemFromDb);
            //InventoryItems.Add(newItemFromDb);
            // sample data (for testing UI)
            _allItems.Add(new Inventory
            {
                Id = 1,
                ItemName = "Milk",
                TotalAmount = 20,
                RetailPrice = 50,
                ItemType = "Dairy",
                LastModified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            });

            _allItems.Add(new Inventory
            {
                Id = 2,
                ItemName = "Bread",
                TotalAmount = 15,
                RetailPrice = 30,
                ItemType = "Bakery",
                LastModified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            });

            foreach (var item in _allItems)
                InventoryItems.Add(item);
            // command for adding item
            AddItemCommand = new Command(AddItem);
            EditCommand = new Command<Inventory>(async (item) => await EditItem(item));
        }

        private void FilterItems(string query)
        {
            InventoryItems.Clear();

            var filtered = string.IsNullOrWhiteSpace(query)
                ? _allItems
                : _allItems.Where(i => i.ItemName.ToLower().Contains(query.ToLower()));

            foreach (var item in filtered)
                InventoryItems.Add(item);
        }
        private void AddItem()
        {
            var newItem = new Inventory
            {
                Id = _allItems.Count + 1,
                ItemName = "New Item",
                TotalAmount = 0,
                RetailPrice = 0,
                ItemType = "Unknown",
                LastModified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            _allItems.Add(newItem);
            InventoryItems.Add(newItem);
        }

        private async Task EditItem(Inventory item)
        {
            if (item == null) return;
            await Application.Current.MainPage.Navigation.PushAsync(new EditInventory(item));
        }
    }
}
