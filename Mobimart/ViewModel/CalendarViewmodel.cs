using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobiMart.Model;
using MobiMart.Service;
using MobiMart.View;
using SQLitePCL;

namespace MobiMart.ViewModel
{
    public partial class CalendarViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string monthYear;
        [ObservableProperty]
        // private ObservableCollection<DateTime> days = new();
        List<CalendarDay> days = [];
        [ObservableProperty]
        List<Reminder> reminders = [];
        [ObservableProperty]
        DateTime currentDate;

        private CalendarDay selectedDay;
        List<Reminder> allReminders = [];


        // private DateTime _currentDate;

        NotificationService notificationService;

        public CalendarViewModel(NotificationService notificationService)
        {
            Title = "Calendar";
            this.notificationService = notificationService;
            CurrentDate = DateTime.Today;
            Days = [];

            // Days = [];
            // for (int i = 0; i < 31; i++)
            // {
            //     Days.Add(new(i));
            // }
            // LoadMonth();

            // debug data
            reminders = [];
            // reminders.Add(new()
            // {
            //     Type = ReminderType.SupplyRunout,
            //     Title = "Stock running low",
            //     Message = "Stock for item Lucky Me Beef Noodles is running low\nRemaning stock: 4",
            //     NotifyAtDate = DateTime.Now.ToString(),
            //     RepeatDaily = true,
            //     IsEnabled = true,
            // });
            // reminders.Add(new()
            // {
            //     Type = ReminderType.SupplyRunout,
            //     Title = "Stock running low",
            //     Message = "Stock for item Rexona Ice Cool is running low\nRemaning stock: 2",
            //     NotifyAtDate = DateTime.Now.ToString(),
            //     RepeatDaily = true,
            //     IsEnabled = true,
            // });
            // reminders.Add(new()
            // {
            //     Type = ReminderType.SupplyRunout,
            //     Title = "Stock running low",
            //     Message = "Stock for item Coke Kasalo is running low\nRemaning stock: 5",
            //     NotifyAtDate = DateTime.Now.ToString(),
            //     RepeatDaily = true,
            //     IsEnabled = true,
            // });
        }

        [RelayCommand]
        private async Task NextMonth()
        {
            CurrentDate = new DateTime(CurrentDate.Year, CurrentDate.Month + 1, 1);
            await LoadMonth();
        }

        [RelayCommand]
        private async Task PrevMonth()
        {
            CurrentDate = new DateTime(CurrentDate.Year, CurrentDate.Month - 1, 1);
            await LoadMonth();
        }

        private async Task LoadMonth()
        {
            // tap day one of the month
            DayTapped(Days[0]);

            // adjust the length of the month
            for (int i = 28; i < Days.Count; i++) Days[i].IsVisible = true;
            var daysInMonth = DateTime.DaysInMonth(CurrentDate.Year, CurrentDate.Month);
            for (int i = 0; i < Days.Count - daysInMonth; i++)
            {
                Days[Days.Count - 1 - i].IsVisible = false;
            }

            UpdateIndicators();
        }       


        private void UpdateIndicators()
        {
            // reset notification indicator for each day that hasReminders
            foreach (var day in Days)
            {
                if (!day.HasReminders) continue;
                day.HasReminders = false;
            }
            // add notification indicator to proper days
            foreach (var reminder in allReminders)
            {
                if (DateTime.Parse(reminder.NotifyAtDate).Month != CurrentDate.Month) continue;
                Days[DateTime.Parse(reminder.NotifyAtDate).Day - 1].HasReminders = true;
            }
        }


        [RelayCommand]
        public void DayTapped(CalendarDay day)
        {
            selectedDay ??= day;
            CurrentDate = new DateTime(CurrentDate.Year, CurrentDate.Month, day.Value);
            selectedDay.IsSelected = false;
            selectedDay = Days[day.DayIdx];
            selectedDay.IsSelected = true;

            Reminders = [.. allReminders.Where(x => DateTime.Parse(x.NotifyAtDate).Date == CurrentDate.Date)];
        }


        [RelayCommand]
        public async Task AddDelivery()
        {
            await Shell.Current.GoToAsync(nameof(AddDeliveryReminder), true);
        }


        public async Task OnAppearing()
        {
            // if (Reminders.Count <= 0)
            // {
            //     allReminders = await notificationService.GetAllRemindersAsync();
            //     Reminders = [.. allReminders.Where(x => x.NotifyAtDate.Equals(""))];
            // }

            allReminders = await notificationService.GetAllRemindersAsync();
            Reminders = [.. allReminders.Where(x => DateTime.Parse(x.NotifyAtDate).Date == DateTime.Now.Date)];

            if (Days.Count <= 0)
            {
                Days = [];
                for (int i = 0; i < 31; i++)
                {
                    Days.Add(new(i));
                }
                Days = [.. Days.AsEnumerable()];
                await LoadMonth();
                return;
            }

            UpdateIndicators();
        }
    }
}