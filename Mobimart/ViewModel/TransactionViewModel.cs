using MobiMart.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MobiMart.ViewModel
{
    class TransactionViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Transaction> Items { get; set; } = new ObservableCollection<Transaction>();

        private decimal totalPrice;
        public decimal TotalPrice
        {
            get => totalPrice;
            set
            {
                totalPrice = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public void AddItem(Transaction item)
        {
            item.OnQuantityOrPriceChanged = UpdateTotal;
            Items.Add(item);
            UpdateTotal();
        }

        private void UpdateTotal()
        {
            TotalPrice = 0;
            foreach (var item in Items)
            {
                TotalPrice += item.Price * item.Quantity;
            }
        }

        public ICommand AddItemCommand => new Command(() =>
        {
            AddItem(new Transaction());
        });

        public ICommand SaveTransactionCommand => new Command(async () =>
        {
            if (Items.Count == 0)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No items to save.", "OK");
                return;
            }

            // Create new record and copy Items
            var record = new TransactionRecord
            {
                Name = $"Transaction {TransactionStore.Records.Count + 1}",
                Date = DateTime.Now,
                TotalPrice = TotalPrice,
                Items = Items.Select(item => new Transaction
                {
                    ItemName = item.ItemName,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList() // Copy into a new list
            };

            TransactionStore.Records.Add(record);

            // Show success popup
            await Application.Current.MainPage.DisplayAlert("Success", "Transaction saved!", "OK");

            // Clear transaction form
            Items.Clear();
            TotalPrice = 0;
        });

    }
}
