namespace MobiMart;

public partial class SignUpPage : ContentPage
{
	public SignUpPage()
	{
		InitializeComponent();
	}
    private async void OnLoginTapped(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}