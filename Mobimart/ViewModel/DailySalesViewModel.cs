using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MobiMart.ViewModel;

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

    public DailySalesViewModel()
    {
        Title = "Daily Sales";
        LoadDailyData(CurrentDate);
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
}