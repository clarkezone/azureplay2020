using System;
using Microsoft.Extensions.Configuration;

namespace DataLayer
{
    public class ServiceDescriptionService : SimpleMongoObjectStore<ServiceDescription>
    {
        public ServiceDescriptionService(IConfiguration configuration) : base(configuration.GetConnectionString("SuperFancyConnectionString"), "AzureServices", "ServiceDescriptions")
        {
        }

        public ServiceDescriptionService(string conn) : base(conn, "AzureServices", "ServiceDescriptions") { }

        
    }
}
