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
            PeopleService ps = new PeopleService(DevConnectionString.GremlinEnpointUrl, DevConnectionString.GremlinPrimaryKey); ;


            //var azureServicesService = new ServiceDescriptionService(ConnectionString.DevMongoConnectionString);
            //InsertDatabase(azureServicesService);
            //var services = azureServicesService.List();
            //PrintItems(services);

           // var aservice = azureServicesService.Get("5e7e790c7396fe1eb826b71d");
            //Console.WriteLine(aservice);
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
