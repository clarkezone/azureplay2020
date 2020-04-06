using DataLayerGremlin;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace azureplaycmdline
{
    class GremlinUtils
    {
        public static async Task InsertAllPeople()
        {
            var ps = new PeopleService(DevConnectionStrings.GremlinEnpointUrl, DevConnectionStrings.GremlinPrimaryKey);
            ps.AddPerson("person", "James", "Clarke");
        }

    }
}
