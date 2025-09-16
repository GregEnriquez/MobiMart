using CommunityToolkit.Maui.Views;

namespace MobiMart.View;

public partial class SaveInventoryPopup : Popup
{
    public SaveInventoryPopup(string message)
    {
        InitializeComponent();
    }

    private void OnCloseClicked(object sender, EventArgs e)
    {
        this.CloseAsync();
    }
}