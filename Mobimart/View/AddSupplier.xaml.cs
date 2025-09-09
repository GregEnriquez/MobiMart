namespace MobiMart.View;

public partial class AddSupplier : ContentPage
{
	public AddSupplier()
	{
		InitializeComponent();
	}

    private async void onButtonClicked(object sender, EventArgs args)
    {
        await Shell.Current.GoToAsync(nameof(AddContacts));
    }
}