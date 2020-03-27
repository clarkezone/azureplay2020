namespace DataLayer
{
    public class AzureServiceDescriptionService : SimpleMongoObjectStore<AzureServiceDescription>
    {
        public AzureServiceDescriptionService(string connectionString, string database, string collection) : base(connectionString, database, collection)
        {
        }
    }
}
