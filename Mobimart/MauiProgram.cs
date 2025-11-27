using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm;
using Microsoft.Extensions.Logging;
using MobiMart.Service;
using MobiMart.View;
using MobiMart.ViewModel;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Maui;
using ZXing.Net.Maui.Controls;
using Plugin.LocalNotification;
using SkiaSharp.Views.Maui.Controls.Hosting;
using CommunityToolkit.Maui.Core;

namespace MobiMart
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMauiCommunityToolkitCamera()
                .UseLocalNotification()
                .UseSkiaSharp()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .UseBarcodeReader();
            builder.Services.AddTransient<AppShell>();

            // inject services
            builder.Services.AddSingleton<UserService>();
            builder.Services.AddSingleton<BusinessService>();
            builder.Services.AddSingleton<UserService>();
            builder.Services.AddSingleton<SupplierService>();
            builder.Services.AddSingleton<InventoryService>();
            builder.Services.AddSingleton<SalesService>();
            builder.Services.AddSingleton<NotificationService>();
            builder.Services.AddSingleton<OpenAiService>();
            builder.Services.AddSingleton<GeminiService>();
            builder.Services.AddSingleton<SyncService>();

            // inject viewmodels
            builder.Services.AddTransient<SignUpViewModel>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<UserPageViewModel>();
            builder.Services.AddTransient<BusinessPageViewModel>();
            builder.Services.AddTransient<FlyoutMenuViewModel>();
            builder.Services.AddTransient<AddSupplierViewModel>();
            builder.Services.AddTransient<CalendarViewModel>();
            builder.Services.AddTransient<DailySalesViewModel>();
            builder.Services.AddTransient<IncomeSummaryViewModel>();
            builder.Services.AddTransient<SalesForecastViewModel>();
            builder.Services.AddTransient<EmployeeTablePageViewModel>();
            builder.Services.AddTransient<SupplierListViewModel>();
            builder.Services.AddTransient<SupplierInfoViewModel>();
            builder.Services.AddTransient<AddSupplierItemViewModel>();
            builder.Services.AddTransient<SupplierInventoryViewModel>();
            builder.Services.AddTransient<InventoryViewModel>();
            builder.Services.AddTransient<EditInventoryPopupViewModel>();
            builder.Services.AddTransient<EditSuppInventoryViewModel>();
            builder.Services.AddTransient<TransactionViewModel>();
            builder.Services.AddTransient<SalesHistoryViewModel>();
            builder.Services.AddTransient<ViewTransactionViewModel>();
            builder.Services.AddTransient<AddDeliveryReminderViewModel>();
            builder.Services.AddTransient<MessageSupplierViewModel>();
            builder.Services.AddTransient<AddContactsViewModel>();
            builder.Services.AddTransient<ContractsViewModel>();

            // inject views
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<SignUpPage>();
            builder.Services.AddTransient<UserPage>();
            builder.Services.AddTransient<BusinessPage>();
            builder.Services.AddTransient<EmployeeTablePage>();
            builder.Services.AddTransient<InventoryListPage>();
            builder.Services.AddTransient<TransactionPage>();
            builder.Services.AddTransient<SalesHistory>();
            builder.Services.AddTransient<SupplierList>();
            builder.Services.AddTransient<EditInventoryPopup>();
            builder.Services.AddTransient<EditSupplierInventory>();
            builder.Services.AddTransient<SyncBusyPopup>();
        

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
