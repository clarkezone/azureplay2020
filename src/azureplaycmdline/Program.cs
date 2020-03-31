using DataLayer;
using MongoDB.Bson;
using System;
using System.Collections;

namespace azureplaycmdline
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString =
    @"mongodb://clarkezonetestcosmosforplay:QV6W2fWmedSoEGvQSVAzP3IOUZWtTyjIkGq2k4Yxna6JEm0lvGm4p7zbcPOMaKYwMzkWCxJTguDw5QYbPh4cng==@clarkezonetestcosmosforplay.mongo.cosmos.azure.com:10255/?ssl=true&replicaSet=globaldb&maxIdleTimeMS=120000&appName=@clarkezonetestcosmosforplay@&retrywrites=false";

            //UserService d = new UserService(connectionString, "People", "PeopleCollection");

            //d.Insert(User.Create("Randle", "Redmond"));
            //var users = d.List();
            //PrintUsers(users);

            var azureServicesService = new AzureServiceDescriptionService(connectionString);
            InsertServices(azureServicesService);
            //var services = azureServicesService.List();
            //PrintItems(services);

            var aservice = azureServicesService.Get("5e7e790c7396fe1eb826b71d");
            Console.WriteLine(aservice);
            Console.WriteLine("Done press return to exit");
            Console.ReadLine();

            //CreateBson();
        }

        private static void InsertServices(AzureServiceDescriptionService azureServicesService)
        {
            var services = new ServiceDescription[] {
            ServiceDescription.CreateAzure("Virtual Machines", AzureServiceType.Compute,AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("Virtual Machines", AzureServiceType.Compute, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("Virtual Machines (classic)", AzureServiceType.Compute, AzureSupportLevel.Classic),
            ServiceDescription.CreateAzure("Virtual Machine scale sets", DataLayer.AzureServiceType.Compute, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("Container services (deprecated)", DataLayer.AzureServiceType.Compute, AzureSupportLevel.Deprecated),
            ServiceDescription.CreateAzure("Function App", DataLayer.AzureServiceType.Compute, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("App Services", DataLayer.AzureServiceType.Compute, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("Container Instances", DataLayer.AzureServiceType.Compute, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("Batch accounts", DataLayer.AzureServiceType.Compute, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("Service Fabric clusters", DataLayer.AzureServiceType.Compute, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("Mesh applications", DataLayer.AzureServiceType.Compute, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("Cloud services (classic)", DataLayer.AzureServiceType.Compute, AzureSupportLevel.Classic),
            ServiceDescription.CreateAzure("Kubernetes services", DataLayer.AzureServiceType.Compute, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("Availability sets", DataLayer.AzureServiceType.Compute, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("Disks", DataLayer.AzureServiceType.Compute, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("Disks (classic)", DataLayer.AzureServiceType.Compute, AzureSupportLevel.Classic),
            ServiceDescription.CreateAzure("Snapshots", DataLayer.AzureServiceType.Compute, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("Images", DataLayer.AzureServiceType.Compute, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("Image definitions", DataLayer.AzureServiceType.Compute, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("Image versions", DataLayer.AzureServiceType.Compute, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("Shared image galleries", DataLayer.AzureServiceType.Compute, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("OS images (classic)", DataLayer.AzureServiceType.Compute, AzureSupportLevel.Classic),
            ServiceDescription.CreateAzure("VM images (classic)", DataLayer.AzureServiceType.Compute, AzureSupportLevel.Classic),
            ServiceDescription.CreateAzure("Proximity palcement groups", DataLayer.AzureServiceType.Compute, AzureSupportLevel.Current),
            ServiceDescription.CreateAzure("Hosts", DataLayer.AzureServiceType.Compute, AzureSupportLevel.Current, new BsonDocument
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
            ServiceDescription.CreateAzure("Host groups", DataLayer.AzureServiceType.Compute, AzureSupportLevel.Current),
            };

            foreach (var item in services)
            {
                azureServicesService.Insert(item);
            }
        
            // [x] move to compact syntax adding is legacy
            // [Add a random details blob with dynamic type json construction and BSON serialization
        
        }

        private static void PrintItems(IList items)
        {
            foreach (var user in items)
            {
                Console.WriteLine(user);
            }
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
