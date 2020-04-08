using DataLayer;
using DataLayerGremlin;
using MongoDB.Bson;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace azureplaycmdline
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //Gremlin playground
            //PeopleService ps = new PeopleService(DevConnectionString.GremlinEnpointUrl, DevConnectionString.GremlinPrimaryKey); ;
            //await GremlinUtils.InsertAllPeople();

            //Mongo playground
            var items = MongoUtils.QueryAzureServices();
            PrintItems(items);

            //MongoUtils.InsertAllAzureServices();

            //CreateBson();

            Console.WriteLine("Done press return to exit");
            Console.ReadLine();
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
