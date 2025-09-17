using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MobiMart.ViewModel
{
    public partial class CalendarViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string monthYear;

        [ObservableProperty]
        private ObservableCollection<DateTime> days = new();

        private DateTime _currentDate;

        public CalendarViewModel()
        {
            Title = "Calendar";
            _currentDate = DateTime.Today;
            LoadMonth(_currentDate);
        }

        [RelayCommand]
        private void NextMonth()
        {
            _currentDate = _currentDate.AddMonths(1);
            LoadMonth(_currentDate);
        }

        [RelayCommand]
        private void PrevMonth()
        {
            _currentDate = _currentDate.AddMonths(-1);
            LoadMonth(_currentDate);
        }

        private void LoadMonth(DateTime date)
        {
            Days.Clear();
            MonthYear = date.ToString("MMMM yyyy");

            var firstDay = new DateTime(date.Year, date.Month, 1);
            var daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);

            for (int i = 0; i < daysInMonth; i++)
            {
                Days.Add(firstDay.AddDays(i));
            }
        }
    }
}