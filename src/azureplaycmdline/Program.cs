using DataLayer;
using DataLayerGremlin;
using MongoDB.Bson;
using System;
using System.Collections;

namespace azureplaycmdline
{
    class Program
    {
        static void Main(string[] args)
        {
            //PeopleService ps = new PeopleService(DevConnectionString.GremlinEnpointUrl, DevConnectionString.GremlinPrimaryKey); ;


            MongoUtils.InsertAllAzureServices();
            Console.WriteLine("Done press return to exit");
            Console.ReadLine();

            //CreateBson();
        }

     

        private static void PrintItems(IList items)
        {
            foreach (var user in items)
            {
                Console.WriteLine(user);
            }
        }

   
    }
}
