﻿using DataLayerMongo;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using System;
using System.Collections.Generic;

namespace azureplaycmdline
{
    class MongoUtils
    {
        public static void InsertAllAzureServices()
        {
            var azureServicesService = new ServiceDescriptionService(DevConnectionStrings.MongoConnectionString);

            azureServicesService.Database.DropCollection("ServiceDescriptions");
            azureServicesService.Database.CreateCollection("ServiceDescriptions");

            azureServicesService.Database.DropCollection("LearningResources");
            azureServicesService.Database.CreateCollection("LearningResources");

            InsertCompute(azureServicesService);
            InsertDatabase(azureServicesService);
        }

        public static void ConfigureCaseInsensitiveIndex()
        {
            var azureServicesService = new ServiceDescriptionService(DevConnectionStrings.MongoConnectionString);
            var collection = azureServicesService.ObjectCollection;
            var keys = Builders<ServiceDescription>.IndexKeys.Ascending("ServiceName");
            CreateIndexOptions indexOptions = new CreateIndexOptions();
            indexOptions.Collation = new Collation("en",strength:CollationStrength.Secondary);
            var model = new CreateIndexModel<ServiceDescription>(keys, indexOptions);
            collection.Indexes.CreateOne(model);
        }

        public static IList<ServiceDescription> QueryCaseInsensitve(string serviceName)
        {
            var azureServicesService = new ServiceDescriptionService(DevConnectionStrings.MongoConnectionString);
            var collection = azureServicesService.ObjectCollection;
            FindOptions fo = new FindOptions();
            fo.Collation = new Collation("en", strength: CollationStrength.Secondary);

            var builder = new FilterDefinitionBuilder<ServiceDescription>();
            var built = builder.Eq<String>((ServiceDescription b) => b.ServiceName, serviceName);
            return collection.Find<ServiceDescription>(built, fo).ToList();

            //collection.Find()
            //return collection.Find<ServiceDescription>(f => f.ServiceName==serviceName, fo).ToList();
        }

        public static void MonitorChanges()
        {
            var azureServicesService = new ServiceDescriptionService(DevConnectionStrings.MongoConnectionString);

            var coll = azureServicesService.ObjectCollection;

            // Example taken from here: https://docs.microsoft.com/en-us/azure/cosmos-db/mongodb-change-streams

            var pipeline = new EmptyPipelineDefinition<ChangeStreamDocument<ServiceDescription>>()
    .Match(change => change.OperationType == ChangeStreamOperationType.Insert || change.OperationType == ChangeStreamOperationType.Update || change.OperationType == ChangeStreamOperationType.Replace)
    .AppendStage<ChangeStreamDocument<ServiceDescription>, ChangeStreamDocument<ServiceDescription>, ServiceDescription>(
    "{ $project: { '_id': 1, 'fullDocument': 1, 'ns': 1, 'documentKey': 1 }}");

            var options = new ChangeStreamOptions
            {
                FullDocument = ChangeStreamFullDocumentOption.UpdateLookup
            };

            var enumerator = coll.Watch(pipeline, options).ToEnumerable().GetEnumerator();

            while (enumerator.MoveNext())
            {
                Console.WriteLine(enumerator.Current);
            }

            enumerator.Dispose();
        }

        internal static List<ServiceDescription> QueryAzureServices()
        {
            var azureServicesService = new ServiceDescriptionService(DevConnectionStrings.MongoConnectionString);
            return azureServicesService.List();
        }

        private static void InsertCompute(ServiceDescriptionService azureServicesService)
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
            ServiceDescription.CreateAzure("Hosts", AzureServiceType.Compute, AzureSupportLevel.Current, new BsonDocument
            {
                { "name", "MongoDB" },
                { "type", "Database" },
                { "count", 1 },
                { "info", new BsonDocument
                    {
                        { "x", 203 },
                        { "y", 102 }
                    }}
            }),
            ServiceDescription.CreateAzure("Host groups", AzureServiceType.Compute, AzureSupportLevel.Current),
            };

            foreach (var item in services)
            {
                azureServicesService.Insert(item);
            }

            // [x] move to compact syntax adding is legacy
            // [Add a random details blob with dynamic type json construction and BSON serialization

        }

        private static void InsertDatabase(ServiceDescriptionService azureServicesService)
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
                azureServicesService.Insert(item);
            }

            // [x] move to compact syntax adding is legacy
            // [Add a random details blob with dynamic type json construction and BSON serialization

        }

        static void CreateBson(string connectionString)
        {
            BsonService d = new BsonService(connectionString, "People", "Things");

            var document = new BsonDocument
            {
                { "name", "MongoDB" },
                { "type", "Database" },
                { "count", 1 },
                { "info", new BsonDocument
                    {
                        { "x", 203 },
                        { "y", 102 }
                    }}
            };
            d.Create(document);
        }
    }
}
