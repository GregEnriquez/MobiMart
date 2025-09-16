using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm;
using Microsoft.Extensions.Logging;
using MobiMart.Service;
using MobiMart.View;
using MobiMart.ViewModel;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Maui;

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
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddSingleton<AppShell>();

            // inject services
            builder.Services.AddSingleton<UserService>();
            builder.Services.AddSingleton<BusinessService>();
            builder.Services.AddSingleton<UserService>();
            builder.Services.AddSingleton<SupplierService>();

            // inject viewmodels
            builder.Services.AddSingleton<LoginViewModel>();
            builder.Services.AddSingleton<SignUpViewModel>();
            builder.Services.AddSingleton<UserPageViewModel>();
            builder.Services.AddSingleton<BusinessPageViewModel>();
            builder.Services.AddSingleton<FlyoutMenuViewModel>();
            builder.Services.AddSingleton<AddSupplierViewModel>();

            // inject views
            builder.Services.AddSingleton<SignUpPage>();
            builder.Services.AddSingleton<UserPage>();
            builder.Services.AddSingleton<BusinessPage>();
            builder.Services.AddSingleton<EmployeeTablePage>();
            builder.Services.AddSingleton<InventoryListPage>();
            builder.Services.AddSingleton<TransactionPage>();
            builder.Services.AddSingleton<SalesHistory>();
            builder.Services.AddSingleton<SupplierList>();
        

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
