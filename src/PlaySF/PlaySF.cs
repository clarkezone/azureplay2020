using System.Collections.Generic;
using System.Fabric;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Data;
using Microsoft.ApplicationInsights.ServiceFabric;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Hosting;
using WebAPIControllers.Extensions;

namespace PlaySF
{
    /// <summary>
    /// The FabricRuntime creates an instance of this class for each service type instance. 
    /// </summary>
    internal sealed class PlaySF : StatefulService
    {
        public PlaySF(StatefulServiceContext context)
            : base(context)
        { }

        /// <summary>
        /// Optional override to create listeners (like tcp, http) for this service instance.
        /// </summary>
        /// <returns>The collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new ServiceReplicaListener[]
            {
                new ServiceReplicaListener(serviceContext =>
                    new KestrelCommunicationListener(serviceContext, (url, listener) =>
                    {

                        ServiceEventSource.Current.ServiceMessage(serviceContext, $"Starting Kestrel on {url}");

                        return new WebHostBuilder()
                                    .UseKestrel()
                                    .UseCommonConfiguration()
                                    .ConfigureAppConfiguration(config => {
                                        //TODO detect when MSI is available and use that
                                        //webBuilder.ConfigureKeyvaultMSI()

                                        var builtConfig = config.Build();

                                        //TODO: these should be constants that map to environment variable config
                                        config.ConfigureKeyvaultAppRegistration($"https://{builtConfig["KeyVaultName"]}.vault.azure.net/",
                                            builtConfig["Secret"], builtConfig["AzureADApplicationId"]);

                                        //config.ConfigureKeyvaultMSI($"https://{builtConfig["KeyVaultName"]}.vault.azure.net/",
                                        //    builtConfig["AzureADApplicationId"]);

                                    })
                                    .ConfigureServices(
                                        services => services
                                        .AddSingleton<ITelemetryInitializer>((serviceProvider) => FabricTelemetryInitializerExtension.CreateFabricTelemetryInitializer(serviceContext))
                                        .AddSingleton<StatefulServiceContext>(serviceContext)
                                        .AddSingleton<IReliableStateManager>(this.StateManager))
                                    .UseContentRoot(Directory.GetCurrentDirectory())
                                    .UseStartup<Startup>()
                                    .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.UseUniqueServiceUrl)
                                    .UseUrls(url)
                                    .Build();
                    }))
            };
        }
    }
}
