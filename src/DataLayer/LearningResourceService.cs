using Microsoft.Extensions.Configuration;

namespace DataLayer
{
    public class LearningResourceService : SimpleMongoObjectStore<LearningResource>
    {
        public LearningResourceService(IConfiguration configuration) : base(configuration.GetConnectionString("SuperFancyConnectionString"), "AzureServices", "LearningResources")
        {
        }

        public LearningResourceService(string conn) : base(conn, "AzureServices", "LearningResources") { }
    }
}
