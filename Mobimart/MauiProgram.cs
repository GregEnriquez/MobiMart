using Microsoft.Extensions.Logging;
using MobiMart.Service;
using MobiMart.View;
using MobiMart.ViewModel;

namespace MobiMart
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddSingleton<UserService>();

            builder.Services.AddSingleton<LoginViewModel>();
            builder.Services.AddSingleton<SignUpViewModel>();
            builder.Services.AddSingleton<UserPageViewModel>();

            builder.Services.AddSingleton<SignUpPage>();
            builder.Services.AddSingleton<UserPage>();
            builder.Services.AddSingleton<BusinessPage>();
            builder.Services.AddSingleton<EmployeeTablePage>();
        

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
