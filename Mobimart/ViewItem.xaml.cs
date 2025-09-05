namespace MobiMart;

public partial class ViewItem : ContentView
{
	public ViewItem()
	{
		InitializeComponent();
	}

    public void SetItemInfo(string name, string quantity, string price, string type, string desc)
    {
        NameLabel.Text = name;
        QuantityLabel.Text = $"Quantity: {quantity}";
        PriceLabel.Text = $"Price: {price}";
        TypeLabel.Text = $"Type: {type}";
        DescLabel.Text = desc;
    }

    private void OnCloseClicked(object sender, EventArgs e)
    {
        this.IsVisible = false;
    }
}