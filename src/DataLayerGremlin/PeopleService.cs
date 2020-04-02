﻿using Gremlin.Net.Driver;
using Gremlin.Net.Structure.IO.GraphSON;

namespace DataLayerGremlin
{
    public class PeopleService
    {
        private static int port = 443;
        private static string database = "peopledb";
        private static string container = "people";

        public PeopleService(string endpointurl, string primarykey)
        {
            var un = "/dbs/" + database + "/colls/" + container;
            var gremlinServer = new GremlinServer(endpointurl, port, enableSsl: true,
                                                   username: un,
                                                   password: primarykey);

            using (var gremlinClient = new GremlinClient(gremlinServer, new GraphSON2Reader(), new GraphSON2Writer(), GremlinClient.GraphSON2MimeType))
            {
            }
        }
    }
}