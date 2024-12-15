using Microsoft.Extensions.Logging;
using DogDatabase;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using DogViewer.Services;

namespace DogViewer
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<DogApiClient>();
            builder.Services.AddSingleton<AlertService>();
            builder.Services.AddDbContext<DbContextDog>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DogContextDB"), 
                options => options.EnableRetryOnFailure(maxRetryCount: 0)), ServiceLifetime.Transient);

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
