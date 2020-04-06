using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Remote;
using Gremlin.Net.Process.Traversal;
using Gremlin.Net.Structure.IO.GraphSON;
using System;
using System.Threading.Tasks;

namespace DataLayerGremlin
{
    public class PeopleService
    {
        private static int port = 443;
        private static string database = "peopledb";
        private static string container = "people";
        private GremlinClient _gremlinClient = null;

        public PeopleService(string endpointurl, string primarykey)
        {
            var un = "/dbs/" + database + "/colls/" + container;
            var gremlinServer = new GremlinServer(endpointurl, port, enableSsl: true,
                                                   username: un,
                                                   password: primarykey);

            //TODO dispose
            _gremlinClient = new GremlinClient(gremlinServer, new GraphSON2Reader(), new GraphSON2Writer(), GremlinClient.GraphSON2MimeType);
        }

        public async void AddPerson(string label, string firstname, string lastname) {

            await _gremlinClient.SubmitAsync<dynamic>($"g.addV('{label}').property('id', '{Guid.NewGuid().ToString()}').property('firstName', '{firstname}').property('lastName', '{lastname}').property('address', 'one')");

            var g = AnonymousTraversalSource.Traversal().WithRemote(new DriverRemoteConnection(_gremlinClient));

            //var v1 = g.AddV(label).Property("id", Guid.NewGuid().ToString()).Property("firstName", firstname).Property("lastNeme", lastname).Property("address", "one").Next();
            //var v1 = g.AddV(label).Property("id", firstname).Property("firstName", firstname).Property("lastNeme", lastname).Property("address", "one").Next();
            var v1 = g.AddV(label).Property("id", firstname).Next();
        }
    }
}
