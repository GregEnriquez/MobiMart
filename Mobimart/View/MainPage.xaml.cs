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
    }
}
