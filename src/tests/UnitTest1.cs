using DataLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace tests
{
    [TestClass]
    public class UnitTest1
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
        public void Serialize()
        {
            //AzureServiceDescriptionService asd = new AzureServiceDescriptionService("");
            ServiceDescription sd = ServiceDescription.CreateAzure("Virtual Machines", AzureServiceType.Compute, AzureSupportLevel.Current);
            var result = System.Text.Json.JsonSerializer.Serialize(sd, typeof(ServiceDescription));
        }
    }
}
