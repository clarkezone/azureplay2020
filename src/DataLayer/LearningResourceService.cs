using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;

namespace DataLayer
{
    public class LearningResourceService : SimpleMongoObjectStore<LearningResource>
    {
        public LearningResourceService(IConfiguration configuration) : base(configuration.GetConnectionString("SuperFancyConnectionString"), "AzureServices", "LearningResources")
        {
        }

        public LearningResourceService(string conn) : base(conn, "AzureServices", "LearningResources") { }

        public void InsertAndUpdateService(LearningResource lr)
        {
            //TODO use DI to retreive the ServiceDescriptionService?
            var serviceDescriptions = Database.GetCollection<ServiceDescription>("ServiceDescriptions");
            using (var session = Client.StartSession())
            {
                session.StartTransaction();

                try
                {
                    ObjectCollection.InsertOne(lr);

                    // Find all learning resources for this service
                    var learningResources = ObjectCollection.Find(f => f.ServiceID == lr.ServiceID);

                    // Build an update definition
                    UpdateDefinition<ServiceDescription> update = Builders<ServiceDescription>.Update.Set(p => p.LearningResources, learningResources.ToList());

                    // TODO: in the future, this should be n most popular
                    // Update the service that we are adding a learning resource for with the full set including the one we added
                    serviceDescriptions.FindOneAndUpdate(f => f.Id == lr.ServiceID, update);

                    session.CommitTransaction();
                }
                catch (Exception)
                {
                    //TODO log message to logging service
                    session.AbortTransaction();
                }
            }
        }
    }
}
