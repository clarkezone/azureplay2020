using DataLayer;
using MongoDB.Driver;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace tests
{
    [TestClass]
    public class MongoTests
    {
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
            var sds = new ServiceDescriptionService(DevConnectionStrings.MongoConnectionString);

            var result = sds.ObjectCollection.Find(p => p.ServiceName == "Host groups");

            LearningResource lr = new LearningResource() { Name = "Cool document", ServiceID = result.FirstOrDefault().Id, Uri = new System.Uri("http://cooldoc") };
            LearningResourceService lrs = new LearningResourceService(DevConnectionStrings.MongoConnectionString);
            lrs.Insert(lr);
        }

        [TestMethod]
        public void CreateLearningResourceWithTransaction()
        {
            var sds = new ServiceDescriptionService(DevConnectionStrings.MongoConnectionString);

            var result = sds.ObjectCollection.Find(p => p.ServiceName == "Host groups");

            LearningResource lr = new LearningResource() { Name = "Cool document", ServiceID = result.FirstOrDefault().Id, Uri = new System.Uri("http://cooldoc") };
            LearningResourceService lrs = new LearningResourceService(DevConnectionStrings.MongoConnectionString);
            bool res = lrs.InsertAndUpdateService(lr);
            if (res==false)
            {
                //TODO how to correctly fail a test?
                throw new System.Exception("Test failed");
            }
        }

        /// <summary>
        /// Ensure we fail if the serviceID isn't valid
        /// </summary>
        [TestMethod]
        public void CreateLearningResourceWithTransactionShouldFail()
        {
            var sds = new ServiceDescriptionService(DevConnectionStrings.MongoConnectionString);

            var result = sds.ObjectCollection.Find(p => p.ServiceName == "Host groups");

            LearningResource lr = new LearningResource() { Name = "Cool document", ServiceID = "", Uri = new System.Uri("http://cooldoc") };
            LearningResourceService lrs = new LearningResourceService(DevConnectionStrings.MongoConnectionString);
            bool res = lrs.InsertAndUpdateService(lr);
            if (res != false)
            {
                //TODO how to correctly fail a test?
                throw new System.Exception("Test failed");
            }
        }


        [TestMethod]
        public void Serialize()
        {
            ServiceDescription sd = ServiceDescription.CreateAzure("Virtual Machines", AzureServiceType.Compute, AzureSupportLevel.Current);
            var result = System.Text.Json.JsonSerializer.Serialize(sd, typeof(ServiceDescription));
        }
    }
}
