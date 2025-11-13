using CommunityToolkit.Maui.Extensions;
using MobiMart.Model;
using MobiMart.ViewModel;
using System;
using System.Diagnostics;

namespace MobiMart.View;

public partial class SalesHistory : ContentPage
{
    private DateTime? _currentDate = null;
    public DateTime CurrentDate
    {
        get => (DateTime)_currentDate!;
        set
        {
            _currentDate = value;
            if (BindingContext is SalesHistoryViewModel vm)
            {
                vm.CurrentDate = value;
            }
        }
    }

    public SalesHistory(SalesHistoryViewModel viewModel)
    {
        InitializeComponent();
        CurrentDate = DateTime.Today;
        BindingContext = viewModel;

        if (BindingContext is SalesHistoryViewModel vm)
        {
            UpdateDateLabel();
        }
    }

    private async Task UpdateDateLabel()
    {
        DateLabel.Text = CurrentDate.ToString("MMMM dd, yyyy");
        await UpdateTransactionList();
    }

    private async void OnPreviousDateClicked(object sender, EventArgs e)
    {
        CurrentDate = CurrentDate.AddDays(-1);
        await UpdateDateLabel();
    }

    private async void OnNextDateClicked(object sender, EventArgs e)
    {
        CurrentDate = CurrentDate.AddDays(1);
        await UpdateDateLabel();
    }

    private async void OnTransactionClicked(object sender, EventArgs e)
    {
        if (sender is Frame frame && frame.BindingContext is SalesRecord record)
        {
            // Pass the transaction record as navigation data
            await Shell.Current.GoToAsync(nameof(ViewTransaction), true,
                new Dictionary<string, object>
                {
                { "SalesRecord", record }
                });
        }
    }

    private async Task UpdateTransactionList()
    {
        if (BindingContext is SalesHistoryViewModel vm)
        {
            await vm.RefreshRecords();
        }
    }


    private async void OnDateLabelTapped(object sender, EventArgs e)
    {
        var popup = new DatePickerPopup(CurrentDate);
        await this.ShowPopupAsync(popup); // show the popup

        var selectedDate = await popup.GetResultAsync(); // get the result

        if (selectedDate.HasValue)
        {
            CurrentDate = selectedDate.Value;
            DateLabel.Text = CurrentDate.ToString("MMMM dd, yyyy");
            await UpdateTransactionList();
        }
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is SalesHistoryViewModel vm)
        {
            await vm.RefreshRecords();
        }
    }

}