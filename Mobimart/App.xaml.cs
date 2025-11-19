using System.Diagnostics;
using MobiMart.Service;
using MobiMart.View;
using MobiMart.ViewModel;
using Plugin.LocalNotification;

namespace MobiMart
{
    public partial class App : Application
    {

        private readonly NotificationService notificationService;
        private readonly InventoryService inventoryService;

        public App(NotificationService notificationService, InventoryService inventoryService)
        {
            InitializeComponent();
            LocalNotificationCenter.Current.NotificationActionTapped += OnLocalNotificationTapped;

            this.notificationService = notificationService;
            this.inventoryService = inventoryService;

            // var services = IPlatformApplication.Current!.Services;
            // var a = services.GetServices<UserService>().First();
            // MainPage = new MainPage(new LoginViewModel(a));
        }

        // protected override Window CreateWindow(IActivationState? activationState)
        // {
        //     return base.CreateWindow(activationState);
        // }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var services = IPlatformApplication.Current!.Services;
            // var appShell = services.GetRequiredService<AppShell>();
            // return new Window(appShell);

            var userService = services.GetServices<UserService>().First();
            return new Window(new MainPage(new LoginViewModel(userService)));
        }


        public static async void SwitchToShell()
        {
            var services = IPlatformApplication.Current!.Services;
            var flyoutMenuVm = services.GetServices<FlyoutMenuViewModel>().First();
            var appshell = new AppShell(flyoutMenuVm); // load the appshell
            Current!.Windows[0].Page = appshell;
        }


        public static void SwitchToLogin()
        {
            var services = IPlatformApplication.Current!.Services;
            var loginPage = services.GetRequiredService<MainPage>();
            Current!.Windows[0].Page = new NavigationPage(loginPage);
        }


        private void OnLocalNotificationTapped(Plugin.LocalNotification.EventArgs.NotificationEventArgs e)
        {
            var data = e.Request.ReturningData; // parse and navigate
            // Dispatch to MainThread and navigate using Shell.Current.GoToAsync(...)
        }


        protected override async void OnStart()
        {
            base.OnStart();
        }
    }
}