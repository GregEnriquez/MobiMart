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
            builder.Services.AddSingleton<AppShell>();

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

            // inject viewmodels
            builder.Services.AddSingleton<LoginViewModel>();
            builder.Services.AddSingleton<SignUpViewModel>();
            builder.Services.AddSingleton<UserPageViewModel>();
            builder.Services.AddSingleton<BusinessPageViewModel>();
            builder.Services.AddSingleton<FlyoutMenuViewModel>();
            builder.Services.AddSingleton<AddSupplierViewModel>();
            builder.Services.AddSingleton<CalendarViewModel>();
            builder.Services.AddSingleton<DailySalesViewModel>();
            builder.Services.AddSingleton<IncomeSummaryViewModel>();
            builder.Services.AddSingleton<SalesForecastViewModel>();
            builder.Services.AddSingleton<EmployeeTablePageViewModel>();
            builder.Services.AddSingleton<SupplierListViewModel>();
            builder.Services.AddSingleton<SupplierInfoViewModel>();
            builder.Services.AddSingleton<AddSupplierItemViewModel>();
            builder.Services.AddSingleton<SupplierInventoryViewModel>();
            builder.Services.AddSingleton<InventoryViewModel>();
            builder.Services.AddSingleton<EditInventoryPopupViewModel>();
            builder.Services.AddSingleton<EditSuppInventoryViewModel>();
            builder.Services.AddSingleton<TransactionViewModel>();
            builder.Services.AddSingleton<SalesHistoryViewModel>();
            builder.Services.AddSingleton<ViewTransactionViewModel>();
            builder.Services.AddSingleton<AddDeliveryReminderViewModel>();
            builder.Services.AddSingleton<MessageSupplierViewModel>();
            builder.Services.AddSingleton<AddContactsViewModel>();
            builder.Services.AddSingleton<ContractsViewModel>();

            // inject views
            builder.Services.AddSingleton<SignUpPage>();
            builder.Services.AddSingleton<UserPage>();
            builder.Services.AddSingleton<BusinessPage>();
            builder.Services.AddSingleton<EmployeeTablePage>();
            builder.Services.AddSingleton<InventoryListPage>();
            builder.Services.AddTransient<TransactionPage>();
            builder.Services.AddSingleton<SalesHistory>();
            builder.Services.AddSingleton<SupplierList>();
            builder.Services.AddTransient<EditInventoryPopup>();
            builder.Services.AddTransient<EditSupplierInventory>();
        

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
