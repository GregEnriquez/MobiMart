using CommunityToolkit.Maui.Extensions;
using MobiMart.Model;
using MobiMart.View;
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
                OnPropertyChanged(nameof(Change));
            }
        }

        private decimal payment;
        public decimal Payment
        {
            get => payment;
            set
            {
                if (payment != value)
                {
                    payment = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Change));
                }
            }
        }
        public decimal Change => Payment - TotalPrice;

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
                var notsuccessPopup = new NoItemsPopup();
                Application.Current.MainPage.ShowPopup(notsuccessPopup);
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
                }).ToList(),
                Payment = this.Payment,
                Change = this.Change
            };

            TransactionStore.Records.Add(record);

            // Show success popup
            var successPopup = new SaveInventoryPopup("Transaction saved!");
            Application.Current.MainPage.ShowPopup(successPopup);

            // Clear transaction form
            Items.Clear();
            TotalPrice = 0;
            Payment = 0;
        });

    }
}
