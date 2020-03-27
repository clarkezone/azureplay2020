using DataLayer;
using MongoDB.Bson;
using System;

namespace azureplaycmdline
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString =
    @"mongodb://clarkezonetestcosmosforplay:QV6W2fWmedSoEGvQSVAzP3IOUZWtTyjIkGq2k4Yxna6JEm0lvGm4p7zbcPOMaKYwMzkWCxJTguDw5QYbPh4cng==@clarkezonetestcosmosforplay.mongo.cosmos.azure.com:10255/?ssl=true&replicaSet=globaldb&maxIdleTimeMS=120000&appName=@clarkezonetestcosmosforplay@&retrywrites=false";

            UserService d = new UserService(connectionString);

            d.Insert(User.Create("Randle", "Redmond"));

            PrintUsers(d);
            Console.ReadLine();

            //CreateBson();
        }

        private static void PrintUsers(UserService d)
        {
            var users = d.List();

            foreach (var user in users)
            {
                Console.WriteLine(user.ToString());
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
