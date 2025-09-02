namespace MobiMart;

public partial class SalesHistory : ContentPage
{
    private DateTime _currentDate = DateTime.Today;
    public SalesHistory()
	{
		InitializeComponent();
        UpdateDateLabel();
    }

    private void OnHamburgerClicked(object sender, EventArgs e)
    {
        Shell.Current.FlyoutIsPresented = true;
    }

    private void UpdateDateLabel()
    {
        DateLabel.Text = _currentDate.ToString("MMMM dd, yyyy");
    }

    private void OnPreviousDateClicked(object sender, EventArgs e)
    {
        _currentDate = _currentDate.AddDays(-1);
        UpdateDateLabel();
    }

    private void OnNextDateClicked(object sender, EventArgs e)
    {
        _currentDate = _currentDate.AddDays(1);
        UpdateDateLabel();
    }

    private async void OnTransactionClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ViewTransaction), true);
    }
}