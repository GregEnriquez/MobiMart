namespace MobiMart.View;

public partial class DailySales : ContentPage
{
    DateTime _currentDate = DateTime.Today;
    public DailySales()
	{
		InitializeComponent();
        UpdateDateLabel();
    }

    private void OnHamburgerClicked(object sender, EventArgs e)
    {
        Shell.Current.FlyoutIsPresented = true;
    }

    void OnPreviousDayClicked(object sender, EventArgs e)
    {
        _currentDate = _currentDate.AddDays(-1);
        UpdateDateLabel();
    }

    void OnNextDayClicked(object sender, EventArgs e)
    {
        _currentDate = _currentDate.AddDays(1);
        UpdateDateLabel();
    }

    void OnDateTapped(object sender, EventArgs e)
    {
        DisplayAlert("Date tapped", $"You tapped on {_currentDate:D}", "OK");
    }

    void UpdateDateLabel()
    {
        CurrentDateLabel.Text = _currentDate.ToString("MMMM dd, yyyy");
    }
}