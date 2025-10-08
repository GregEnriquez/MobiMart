using MobiMart.ViewModel;

namespace MobiMart.View;

public partial class AddDeliveryReminder : ContentPage
{
	public AddDeliveryReminder(AddDeliveryReminderViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}


	protected override async void OnAppearing()
	{
		base.OnAppearing();

		if (BindingContext is AddDeliveryReminderViewModel vm)
		{
			await vm.OnAppearing();
		}
    }
}