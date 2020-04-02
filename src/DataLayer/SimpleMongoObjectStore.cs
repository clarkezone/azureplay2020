using MongoDB.Driver;
using System.Collections.Generic;
using System.Security.Authentication;

namespace DataLayer
{
    public class SimpleMongoObjectStore<T> where T : IObjectID
    {
        readonly string _connectionString;

        public bool BadConnectionString()
        {
            return string.IsNullOrEmpty(_connectionString);
        }

        public IMongoCollection<T> ObjectCollection { get; }

        protected MongoClient Client { get; }

        public IMongoDatabase Database { get; }

        public SimpleMongoObjectStore(string connectionString, string database, string collection)
        {
            MongoClientSettings settings = MongoClientSettings.FromUrl(
              new MongoUrl(connectionString)
            );
            settings.SslSettings =
              new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            Client = new MongoClient(settings);

            Database = Client.GetDatabase(database);
            ObjectCollection = Database.GetCollection<T>(collection);
            _connectionString = connectionString;
        }

        public List<T> List() => ObjectCollection.Find(p => true).ToList();

        public void Insert(T u) => ObjectCollection.InsertOne(u);

        public T Get(T id) => ObjectCollection.Find<T>(u => u.Id == ((IObjectID)id).Id).FirstOrDefault();
        public T Get(string id) => ObjectCollection.Find(u => u.Id == id).FirstOrDefault();

        public void Delete(T @object) => ObjectCollection.DeleteOne<T>(u => u.Id == @object.Id);

        public void Update(string id, T value)
        {
            ObjectCollection.ReplaceOne<T>(u => u.Id == id, value);
        }
    }
}
