namespace MobiMart.View;

public partial class Calendar : ContentPage
{
    private DateTime _currentMonth;

    public Calendar()
    {
        InitializeComponent();
        _currentMonth = DateTime.Now;
        GenerateCalendar(_currentMonth);
    }

    private void GenerateCalendar(DateTime date)
    {
        CalendarGrid.Children.Clear();
        CalendarGrid.RowDefinitions.Clear();

        // Always add a row for weekdays
        CalendarGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        // Update month label
        MonthLabel.Text = date.ToString("MMMM yyyy");

        // Add weekday headers
        string[] weekdays = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
        for (int i = 0; i < 7; i++)
        {
            var lbl = new Label
            {
                Text = weekdays[i],
                HorizontalOptions = LayoutOptions.Center,
                TextColor = Colors.Black,
                FontAttributes = FontAttributes.Bold
            };
            CalendarGrid.Add(lbl, i, 0);
        }

        // Get month details
        DateTime firstDay = new DateTime(date.Year, date.Month, 1);
        int daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);

        int row = 1;
        int col = (int)firstDay.DayOfWeek;

        CalendarGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        // Fill day numbers
        for (int day = 1; day <= daysInMonth; day++)
        {
            if (col == 0 && day != 1)
            {
                row++;
                CalendarGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }

            var lbl = new Label
            {
                Text = day.ToString(),
                HorizontalOptions = LayoutOptions.Center,
                TextColor = Colors.Black
            };

            // Highlight today
            if (date.Year == DateTime.Now.Year &&
                date.Month == DateTime.Now.Month &&
                day == DateTime.Now.Day)
            {
                lbl.BackgroundColor = Colors.Gold;
                lbl.FontAttributes = FontAttributes.Bold;
            }

            CalendarGrid.Add(lbl, col, row);

            col = (col + 1) % 7;
        }
    }

    private void OnPrevMonthClicked(object sender, EventArgs e)
    {
        _currentMonth = _currentMonth.AddMonths(-1);
        GenerateCalendar(_currentMonth);
    }

    private void OnNextMonthClicked(object sender, EventArgs e)
    {
        _currentMonth = _currentMonth.AddMonths(1);
        GenerateCalendar(_currentMonth);
    }
}