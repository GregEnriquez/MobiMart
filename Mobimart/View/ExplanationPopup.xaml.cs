using CommunityToolkit.Maui.Views;

namespace MobiMart.View;

public partial class ExplanationPopup : Popup
{
	public ExplanationPopup(string explanation) : this ("Calculation Details", explanation) {}


	public ExplanationPopup(string title, string explanation)
    {
		InitializeComponent();

		TitleLabel.Text = title;
		ExplanationLabel.Text = explanation;
    }

	private async void OnCloseClicked(object sender, EventArgs e)
    {
        await CloseAsync();
    }
}