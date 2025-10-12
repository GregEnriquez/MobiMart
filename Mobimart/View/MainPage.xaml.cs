using MobiMart.ViewModel;

namespace MobiMart.View
{
    public partial class MainPage : ContentPage
    {
        public MainPage(LoginViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }



        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is LoginViewModel vm)
            {
                await vm.OnAppearing();
            }
        }
    }
}
