using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Text;

namespace DataLayer
{
    public class SimpleMongoObjectStore<T> where T : IObjectID
    {
        readonly IMongoCollection<T> _userCollection;

        public SimpleMongoObjectStore(string connectionString, string database, string collection)
        {
            MongoClientSettings settings = MongoClientSettings.FromUrl(
              new MongoUrl(connectionString)
            );
            settings.SslSettings =
              new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            MongoClient client = new MongoClient(settings);

            var db = client.GetDatabase(database);
            _userCollection = db.GetCollection<T>(collection);
        }

        public List<T> List() => _userCollection.Find(p => true).ToList();

        public void Insert(T u) => _userCollection.InsertOne(u);

        public T Get(T id) => _userCollection.Find<T>(u => u.Id == ((IObjectID)id).Id).FirstOrDefault();

        public void Delete(User user) => _userCollection.DeleteOne<T>(u => u.Id == user.Id);
    }
}
