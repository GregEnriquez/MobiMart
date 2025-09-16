using CommunityToolkit.Maui.Views;

namespace MobiMart.View;

public partial class NoItemsPopup : Popup
{
	public NoItemsPopup()
	{
		InitializeComponent();
	}

    private void OnCloseClicked(object sender, EventArgs e)
    {
        this.CloseAsync();
    }
}