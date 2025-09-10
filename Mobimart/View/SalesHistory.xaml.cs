using MobiMart.Model;

namespace MobiMart.View;

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
        UpdateTransactionList();
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
        if (sender is Frame frame && frame.BindingContext is TransactionRecord record)
        {
            // Pass the transaction record as navigation data
            await Shell.Current.GoToAsync(nameof(ViewTransaction), true,
                new Dictionary<string, object>
                {
                { "TransactionRecord", record }
                });
        }
    }

    private void UpdateTransactionList()
    {
        var filtered = TransactionStore.Records
            .Where(r => r.Date.Date == _currentDate.Date)
            .ToList();

        TransactionList.ItemsSource = filtered;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        UpdateTransactionList();
    }

}