namespace MobiMart.View;

public partial class SupplierList : ContentPage
{
	public SupplierList()
	{
		InitializeComponent();

	}

    private async void onButtonClicked(object sender, EventArgs args)
    {
        await Shell.Current.GoToAsync(nameof(AddSupplier));
    }
}