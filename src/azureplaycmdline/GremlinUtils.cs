using DataLayerGremlin;

namespace azureplaycmdline
{
    class GremlinUtils
    {
        public static void InsertAllPeople()
        {
            var ps = new PeopleService(DevConnectionStrings.GremlinEnpointUrl, DevConnectionStrings.GremlinPrimaryKey);
            ps.AddPerson("person", "James", "Clarke");
        }

    }
}
