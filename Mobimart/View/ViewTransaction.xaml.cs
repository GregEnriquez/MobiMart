using MobiMart.Model;

namespace MobiMart.View;

[QueryProperty(nameof(Record), "TransactionRecord")]
public partial class ViewTransaction : ContentPage
{
	public ViewTransaction()
	{
		InitializeComponent();
	}

    private TransactionRecord _record;
    public TransactionRecord Record
    {
        get => _record;
        set
        {
            _record = value;
            LoadTransaction();
        }
    }

    private void LoadTransaction()
    {
        if (_record == null) return;

        // Example: Populate labels
        // DateLabel.Text = _record.Date.ToString("f");
        // TotalLabel.Text = _record.TotalPrice.ToString("C");

        // ItemsList.ItemsSource = _record.Items;
        ItemsList.ItemsSource = _record.Items;
        TotalLabel.Text = $"Total Price: Php {_record.TotalPrice:N2}";
        PaymentLabel.Text = $"Payment: Php {_record.Payment:N2}";
        ChangeLabel.Text = $"Change: Php {_record.Change:N2}";
    }

    private void OnHamburgerClicked(object sender, EventArgs e)
    {
        Shell.Current.FlyoutIsPresented = true;
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}