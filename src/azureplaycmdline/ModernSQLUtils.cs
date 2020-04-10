using DataLayerModernSQL;
using DataLayerModernSQL.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace azureplaycmdline
{
    class ModernSQLUtils
    {
        public static void InsertAllAzureServices()
        {
            using (var azureServicesService = new DataService(DevConnectionStrings.ModernSqlConnectionString))
            {

                var result = azureServicesService.Database.EnsureDeleted();
                if (result == false)
                {
                    throw new Exception("couldn't delete database");
                }

                azureServicesService.Database.Migrate();
                if (result == false)
                {
                    throw new Exception("couldn't delete database");
                }

                InsertCompute(azureServicesService);
                InsertDatabase(azureServicesService);
            }
        }


        internal static IEnumerable<ServiceDescription> QueryAzureServices()
        {
            var azureServicesService = new DataService(DevConnectionStrings.ModernSqlConnectionString);
            return azureServicesService.Services.OrderBy(ob => ob.ServiceName).AsEnumerable();
        }

        private static void InsertCompute(DataService azureServicesService)
        {
            var services = new ServiceDescription[] {
            ServiceDescription.CreateAzure("Virtual Machines", AzureServiceType.Compute,AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("Virtual Machines", AzureServiceType.Compute, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("Virtual Machines (classic)", AzureServiceType.Compute, AzureSupportLevel.Classic),
            ServiceDescription.CreateAzure("Virtual Machine scale sets", AzureServiceType.Compute, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("Container services (deprecated)", AzureServiceType.Compute, AzureSupportLevel.Deprecated),
            ServiceDescription.CreateAzure("Function App", AzureServiceType.Compute, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("App Services", AzureServiceType.Compute, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("Container Instances", AzureServiceType.Compute, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("Batch accounts", AzureServiceType.Compute, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("Service Fabric clusters", AzureServiceType.Compute, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("Mesh applications", AzureServiceType.Compute, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("Cloud services (classic)", AzureServiceType.Compute, AzureSupportLevel.Classic),
            ServiceDescription.CreateAzure("Kubernetes services", AzureServiceType.Compute, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("Availability sets", AzureServiceType.Compute, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("Disks", AzureServiceType.Compute, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("Disks (classic)", AzureServiceType.Compute, AzureSupportLevel.Classic),
            ServiceDescription.CreateAzure("Snapshots", AzureServiceType.Compute, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("Images", AzureServiceType.Compute, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("Image definitions", AzureServiceType.Compute, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("Image versions", AzureServiceType.Compute, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("Shared image galleries", AzureServiceType.Compute, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("OS images (classic)", AzureServiceType.Compute, AzureSupportLevel.Classic),
            ServiceDescription.CreateAzure("VM images (classic)", AzureServiceType.Compute, AzureSupportLevel.Classic),
            ServiceDescription.CreateAzure("Proximity palcement groups", AzureServiceType.Compute, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("Hosts", AzureServiceType.Compute, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("Host groups", AzureServiceType.Compute, AzureSupportLevel.Current),
            };

            foreach (var item in services)
            {
                azureServicesService.Services.Add(item);
            }
            
            azureServicesService.SaveChanges();
            // [x] move to compact syntax adding is legacy
            // [Add a random details blob with dynamic type json construction and BSON serialization

        }

        private static void InsertDatabase(DataService azureServicesService)
        {
            var services = new ServiceDescription[] {
            ServiceDescription.CreateAzure("Azure Cosmos DB", AzureServiceType.Database,AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("Azure SQL", AzureServiceType.Database, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("SQL databases", AzureServiceType.Database, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("Azure Database for MySQL servers", AzureServiceType.Database, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("Azure Database for PostgreSQL", AzureServiceType.Database, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("Azure Database for MariaDB servers", AzureServiceType.Database, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("SQL servers", AzureServiceType.Database, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("Azure Synapse Analytics (formerly SQL DW)", AzureServiceType.Database, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("Azure Database Migration Services", AzureServiceType.Database, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("Azure Cache for Redis", AzureServiceType.Database, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("SQL Server stretch databases", AzureServiceType.Database, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("Data factories", AzureServiceType.Database, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("SQL elastic pools", AzureServiceType.Database, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("Virtual clusters", AzureServiceType.Database, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("Managed databases", AzureServiceType.Database, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("Elastic Job agents", AzureServiceType.Database, AzureSupportLevel.Preview),
            ServiceDescription.CreateAzure("SQL managed instances", AzureServiceType.Database, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("SQL virtual machines", AzureServiceType.Database, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("SQL Server registries", AzureServiceType.Database, AzureSupportLevel.Current),

            };

            foreach (var item in services)
            {
                azureServicesService.Services.Add(item);
            }

            // [x] move to compact syntax adding is legacy
            // [Add a random details blob with dynamic type json construction and BSON serialization

        }
    }
}
