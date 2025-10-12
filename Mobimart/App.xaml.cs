using System.Diagnostics;
using MobiMart.Service;
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
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var services = IPlatformApplication.Current!.Services;
            var appShell = services.GetRequiredService<AppShell>();
            return new Window(appShell);
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