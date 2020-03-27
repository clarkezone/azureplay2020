using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void InitDb()
        {
            DAL d = new DAL();
            d.Init();
            d.GetPeople();
        }

        [TestMethod]
        public void CreateUser()
        {
            DAL d = new DAL();
            d.Init();
            d.CreatePerson(User.Create("James", "Redmond"));
        }
    }
}
