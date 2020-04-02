using DataLayer;
using MongoDB.Driver;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace tests
{
    [TestClass]
    public class UnitTest1
    {
        string _connectionString = "mongodb://clarkezonetestcosmosforplay:L9hg579CaocxeEwGYxI9JDpUkGYMjS6N30ygXduMdonsroBF6wdyEPjF21hktt9PeIFl0pLmNmI0WkWgDrnBTw==@clarkezonetestcosmosforplay.mongo.cosmos.azure.com:10255/?ssl=true&replicaSet=globaldb&retrywrites=false&maxIdleTimeMS=120000&appName=@clarkezonetestcosmosforplay@&retrywrites=false";

        [TestMethod]
        public void InitDb()
        {
            //DAL d = new DAL();
            //d.Init();
            //d.GetPeople();
        }

        [TestMethod]
        public void CreateUser()
        {
            //AzureServiceDescriptionService d = new AzureServiceDescriptionService("");
            //d.Init();
            //d.CreatePerson(User.Create("James", "Redmond"));
        }

        [TestMethod]
        public void CreateLearningResource()
        {
            var sds = new ServiceDescriptionService(_connectionString);

            var result = sds.ObjectCollection.Find(p => p.ServiceName == "Host groups");

            LearningResource lr = new LearningResource() { Name = "Cool document", ServiceID = result.FirstOrDefault().Id, Uri = new System.Uri("http://cooldoc") };
            LearningResourceService lrs = new LearningResourceService(_connectionString);
            lrs.Insert(lr);
        }

        [TestMethod]
        public void Serialize()
        {
            ServiceDescription sd = ServiceDescription.CreateAzure("Virtual Machines", AzureServiceType.Compute, AzureSupportLevel.Current);
            var result = System.Text.Json.JsonSerializer.Serialize(sd, typeof(ServiceDescription));
        }
    }
}
