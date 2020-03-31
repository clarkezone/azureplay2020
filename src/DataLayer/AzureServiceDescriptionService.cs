using System;
using Microsoft.Extensions.Configuration;

namespace DataLayer
{
    public class AzureServiceDescriptionService : SimpleMongoObjectStore<ServiceDescription>
    {
        public AzureServiceDescriptionService(IConfiguration configuration) : base(configuration.GetConnectionString("SuperFancyConnectionString"), "AzureServices", "ServiceDescriptions")
        {
        }

        
    }
}
