using System;

namespace azureplaycmdline
{
    static class DevConnectionString
    {
        public static string DevMongoConnectionString
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
                return Environment.GetEnvironmentVariable("AZUREPLAY_GREMLIN_ENDPOINTURL");
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
