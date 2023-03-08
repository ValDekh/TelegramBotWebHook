using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using TelegramBotWebHook.ServiceCosmosDB;

[assembly: FunctionsStartup(typeof(TelegramBotWebHook.Startup))]
namespace TelegramBotWebHook
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            
            //builder.Services.AddHttpClient();
            builder.Services.AddScoped<ICosmosDBSetter, CosmosDBSetter>();
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            FunctionsHostBuilderContext context = builder.GetContext();
            builder.ConfigurationBuilder
                .AddJsonFile(Path.Combine(context.ApplicationRootPath, "appsettings.json"), optional: true, reloadOnChange: false);
        }
    }
}
