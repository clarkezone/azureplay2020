using System;

namespace tests
{
    static class DevConnectionStrings
    {
        public static string MongoConnectionString
        {
            get
            {
                return Environment.GetEnvironmentVariable("AZUREPLAY_MONGO_CONNECTIONSTRING");
            }
        }

        public static string GremlinEnpointUrl
        {
            get
            {
                return Environment.GetEnvironmentVariable("AZUREPLAY_GREMLIN_HOSTNAME");
            }
        }

        public static string GremlinPrimaryKey
        {
            get
            {
                return Environment.GetEnvironmentVariable("AZUREPLAY_GREMLIN_PRIMARYKEY");
            }
        }
    }
}
