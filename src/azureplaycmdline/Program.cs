using DataLayerMongo;
using DataLayerGremlin;
using MongoDB.Bson;
using System;
using System.Collections;
using System.Threading.Tasks;
using WebAPIControllers;
using WebAPIControllers.Extensions;

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

            //MongoUtils.ConfigureCaseInsensitiveIndex();

            //var items = MongoUtils.QueryAzureServices();
            //var items = MongoUtils.QueryCaseInsensitve("sql server registries");
            //PrintItems(items);

            //MongoUtils.MonitorChanges();

            //MongoUtils.InsertAllAzureServices();

            //CreateBson();

            //ModernSQLUtils.InsertAllAzureServices();

            await KeyvaultSecrets.GetAKVSecretsUsingCert();

            Console.WriteLine("Done press return to exit");
            Console.ReadLine();
        }



        private static void PrintItems(System.Collections.Generic.IList<ServiceDescription> items)
        {
            foreach (var user in items)
            {
                Console.WriteLine(user);
            }
        }

   
    }
}
