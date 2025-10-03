using CommunityToolkit.Maui.Views;

namespace MobiMart.View;

public partial class DatePickerPopup : Popup
{
    private TaskCompletionSource<DateTime?> _tcs;

    public DatePickerPopup(DateTime currentDate)
    {
        InitializeComponent();
        Picker.Date = currentDate.Date;
        Picker.BackgroundColor = Color.Parse("Transparent");
        _tcs = new TaskCompletionSource<DateTime?>();
    }

    // Called when OK is clicked
    private void OnOkClicked(object sender, EventArgs e)
    {
        _tcs.TrySetResult(Picker.Date); // return the selected date
        this.CloseAsync();
    }

    // Called when Cancel is clicked
    private void OnCancelClicked(object sender, EventArgs e)
    {
        _tcs.TrySetResult(null); // return null if cancelled
        this.CloseAsync();
    }

    // Expose a Task to await
    public Task<DateTime?> GetResultAsync() => _tcs.Task;
}