using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microcharts;
using MobiMart.Model;
using MobiMart.Service;
using SkiaSharp;

namespace MobiMart.ViewModel
{

    public partial class DailySalesViewModel : BaseViewModel
    {
        [ObservableProperty]
        private DateTime currentDate = DateTime.Today;
        [ObservableProperty]
        private decimal previousBalance;
        [ObservableProperty]
        private decimal todaysIncome;
        [ObservableProperty]
        private decimal todaysExpenses;
        [ObservableProperty]
        Chart lineChart;
        [ObservableProperty]
        int lineChartWidth = 0;
        [ObservableProperty]
        Chart pieChart;
        [ObservableProperty]
        Chart pieChartExpense;
        [ObservableProperty]
        List<ItemSold> itemsSold;

        [ObservableProperty] bool hasNoExpenses = false;
        [ObservableProperty] bool hasNoTransactions = false;

        InventoryService inventoryService;
        SalesService salesService;
        SupplierService supplierService;

        public DailySalesViewModel(SalesService salesService, InventoryService inventoryService, SupplierService supplierService)
        {
            Title = "Daily Sales";
            LoadDailyData(CurrentDate);

            this.salesService = salesService;
            this.inventoryService = inventoryService;
            this.supplierService = supplierService;
        }

        [RelayCommand]
        private void PreviousDay()
        {
            CurrentDate = CurrentDate.AddDays(-1);
            LoadDailyData(CurrentDate);
        }

        [RelayCommand]
        private void NextDay()
        {
            CurrentDate = CurrentDate.AddDays(1);
            LoadDailyData(CurrentDate);
        }

        [RelayCommand]
        private async void SelectDate()
        {

            await App.Current!.MainPage!.DisplayAlert(
                "Date tapped",
                $"You tapped on {CurrentDate:D}",
                "OK");
        }

        private void LoadDailyData(DateTime date)
        {
            // Mock values
            PreviousBalance = 5000m;
            TodaysIncome = new Random().Next(1000, 5000);
            TodaysExpenses = new Random().Next(500, 3000);
        }


        public async Task GenerateLineChart()
        {
            var entries = new List<ChartEntry>();
            DateTime date = DateTime.Now;
            int maxValue = 500;
            // get daily total income for the past week
            for (int i = 0; i < 7; i++)
            {
                date = DateTime.Now.AddDays(-i);
                var sales = await salesService.GetSalesRecordsAsync(date);
                float total = 0;
                if (sales is not null && sales.Count > 0)
                {
                    foreach (var sale in sales) total += sale.TotalPrice;
                }

                while (total > maxValue) maxValue += 500;

                entries.Add(new ChartEntry(total)
                {
                    Label = $"{date:M}",
                    ValueLabel = $"₱{total:0.00}",
                    Color = GetRandomColor()
                });
            }

            LineChartWidth = entries.Count * 80;
            entries.Reverse();
            LineChart = new LineChart
            {
                Entries = entries,
                LineMode = LineMode.Straight,
                LineSize = 8,
                PointMode = PointMode.Circle,
                PointSize = 18,
                LabelTextSize = 32f,
                BackgroundColor = SKColor.Parse("#FFFBE9"),
                ShowYAxisText = true,
                YAxisPosition = Position.Left,
                ValueLabelOrientation = Orientation.Horizontal,
                ValueLabelTextSize = 32f,
                LabelOrientation = Orientation.Horizontal,
                SerieLabelTextSize = 32f,
                MinValue = 0,
                MaxValue = maxValue
            };
        }


        public async Task GeneratePieChart()
        {
            var entries = new List<ChartEntry>();
            var sales = await salesService.GetSalesRecordsAsync(DateTime.Now);
            float total = 0;
            if (sales is null || sales.Count <= 0) { HasNoTransactions = true; return; }
            var itemsSold = new Dictionary<String, float>(); // barcode, soldAmount terms of price

            ItemsSold = [];
            foreach (var sale in sales)
            {
                total += sale.TotalPrice;
                foreach (var item in sale.Items)
                {
                    if (itemsSold.ContainsKey(item.Barcode))
                    {
                        itemsSold[item.Barcode] += item.Price;
                        // var i = ItemsSold.Find(x => x.Barcode.Equals(item.Barcode))!;
                        // i.Amount += item.Quantity;
                        // i.Total += item.Price;
                        continue;
                    }
                    itemsSold.Add(item.Barcode, item.Price);
                    ItemsSold.Add(new()
                    {
                        ItemName = item.Name,
                        Barcode = item.Barcode,
                        Amount = item.Quantity,
                        Total = item.Price,
                        Date = sale.Date
                    });
                }
            }
            ItemsSold = [.. ItemsSold.AsEnumerable()];

            var itemDescs = await inventoryService.GetAllItemsAsync();
            foreach (var itemSold in itemsSold)
            {
                var item = itemDescs.Find(x => x.Barcode.Equals(itemSold.Key))!;
                entries.Add(new ChartEntry(itemSold.Value)
                {
                    Label = item.Name,
                    ValueLabel = $" {itemSold.Value:0.00} | {(itemSold.Value / total) * 100:0}%",
                    Color = GetRandomColor()
                });
            }

            HasNoTransactions = false;
            TodaysIncome = (decimal)total;
            PieChart = new PieChart
            {
                Entries = entries,
                LabelTextSize = 32f,
                LabelMode = LabelMode.RightOnly,
                BackgroundColor = SKColor.Parse("#FFFBE9"),
            };
        }


        public async Task GeneratePieChartExpense()
        {
            var entries = new List<ChartEntry>();
            var deliveries = await inventoryService.GetDeliveriesViaDate(DateTime.Now);
            float total = 0;
            var suppliersDelivered = new Dictionary<string, float>(); // supplier, expense

            var suppliers = await supplierService.GetAllSuppliersAsync();

            foreach (var delivery in deliveries)
            {
                var supplier = suppliers.Find(x => x.Id == delivery.SupplierId)!;
                if (supplier.Type.ToLower().Equals("consignment")) continue;

                total += delivery.BatchWorth;
                if (suppliersDelivered.ContainsKey(supplier.Name))
                {
                    suppliersDelivered[supplier.Name] += delivery.BatchWorth;
                    continue;
                }
                suppliersDelivered.Add(supplier.Name, delivery.BatchWorth);
            }

            foreach (var x in suppliersDelivered)
            {
                entries.Add(new ChartEntry(x.Value)
                {
                    Label = x.Key,
                    ValueLabel = $"₱{x.Value:0.00} | {(x.Value / total) * 100:0}%",
                    Color = GetRandomColor()
                });
            }

            HasNoExpenses = entries.Count <= 0;
            TodaysExpenses = (decimal)total;
            PieChartExpense = new PieChart
            {
                Entries = entries,
                LabelTextSize = 32f,
                LabelMode = LabelMode.RightOnly,
                BackgroundColor = SKColor.Parse("#FFFBE9"),
            };

        }


        public async Task OnAppearing()
        {
            await GenerateLineChart();
            await GeneratePieChart();
            await GeneratePieChartExpense();
        }



        private static SKColor GetRandomColor(bool isOpaque = false)
        {
            Random _random = new();
            // Generate random values for Red, Green, Blue, and Alpha (0-255)
            byte red = (byte)_random.Next(256);
            byte green = (byte)_random.Next(256);
            byte blue = (byte)_random.Next(256);
            byte alpha = (byte)_random.Next(256);

            // Create a new Color object from the random components
            return isOpaque ? new SKColor(red, green, blue, 255) : new SKColor(red, green, blue, alpha);
            // return SKColor.Parse(Color.FromRgba(red, green, blue, alpha).ToRgbaString());
        }
    }
    
    public class ItemSold
    {
        public string Barcode { get; set; }
        public string ItemName { get; set; }
        public int Amount { get; set; }
        public double Total { get; set; }
        public string Date { get; set; }
        public string ItemType { get; set; }
    }
}
