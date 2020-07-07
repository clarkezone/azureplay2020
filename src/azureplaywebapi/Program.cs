using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebAPIControllers.Extensions;

namespace azureplaywebapi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(config => {
                    //TODO detect when MSI is available and use that
                    //webBuilder.ConfigureKeyvaultMSI()
                    var builtConfig = config.Build();

                    config.ConfigureKeyvaultAppRegistration($"https://{builtConfig["KeyVaultName"]}.vault.azure.net/",
                        builtConfig["Secret"], builtConfig["AzureADApplicationId"]);

                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
