using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Security.Authentication;

namespace DataLayer
{
    public class SimpleMongoObjectStore<T> where T : IObjectID
    {
        readonly IMongoCollection<T> _objectCollection;
        readonly string _connectionString;

        public bool BadConnectionString()
        {
            return string.IsNullOrEmpty(_connectionString);
        }

        public SimpleMongoObjectStore(string connectionString, string database, string collection)
        {
            MongoClientSettings settings = MongoClientSettings.FromUrl(
              new MongoUrl(connectionString)
            );
            settings.SslSettings =
              new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            MongoClient client = new MongoClient(settings);

            var db = client.GetDatabase(database);
            _objectCollection = db.GetCollection<T>(collection);
            _connectionString = connectionString;
        }

        public List<T> List() => _objectCollection.Find(p => true).ToList();

        public void Insert(T u) => _objectCollection.InsertOne(u);

        public T Get(T id) => _objectCollection.Find<T>(u => u.Id == ((IObjectID)id).Id).FirstOrDefault();
        public T Get(string id) => _objectCollection.Find(u => u.Id == new ObjectId(id)).FirstOrDefault();

        public void Delete(T @object) => _objectCollection.DeleteOne<T>(u => u.Id == @object.Id);

        public void Update(string id, T value)
        {
            _objectCollection.ReplaceOne<T>(u => u.Id == new ObjectId(id), value);
        }
    }
}
