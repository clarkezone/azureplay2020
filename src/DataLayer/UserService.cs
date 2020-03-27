namespace DataLayer
{
    public class UserService : SimpleMongoObjectStore<User>
    {
        public UserService(string connectionString, string database, string collection) : base(connectionString, database, collection)
        {
        }
    }
}