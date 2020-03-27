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

            var azureServicesService = new AzureServiceDescriptionService(connectionString, "AzureServices", "ServiceDescriptions");
            InsertServices(azureServicesService);
            var services = azureServicesService.List();
            PrintItems(services);
            
            Console.ReadLine();

            //CreateBson();
        }

        private static void InsertServices(AzureServiceDescriptionService azureServicesService)
        {
            azureServicesService.Insert(AzureServiceDescription.Create("Virtual Machines", DataLayer.AzureServiceType.Compute));
            azureServicesService.Insert(AzureServiceDescription.Create("Virtual Machines (classic)", DataLayer.AzureServiceType.Compute));
            azureServicesService.Insert(AzureServiceDescription.Create("Virtual Machine scale sets", DataLayer.AzureServiceType.Compute));
            azureServicesService.Insert(AzureServiceDescription.Create("Container services (deprecated)", DataLayer.AzureServiceType.Compute));
            azureServicesService.Insert(AzureServiceDescription.Create("Function App", DataLayer.AzureServiceType.Compute));
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
