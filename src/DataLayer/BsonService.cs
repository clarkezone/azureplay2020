using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Security.Authentication;

namespace DataLayerMongo
{
    public class BsonService
    {
        IMongoCollection<BsonDocument> _userCollectionBson;

        public BsonService(string connectionString, string dbname, string collectionname)
        {
            MongoClientSettings settings = MongoClientSettings.FromUrl(
              new MongoUrl(connectionString)
            );
            settings.SslSettings =
              new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            MongoClient client = new MongoClient(settings);
            var db = client.GetDatabase(dbname);
            _userCollectionBson = db.GetCollection<BsonDocument>(collectionname);
        }

        public List<BsonDocument> List() => _userCollectionBson.Find(p => true).ToList();


        public void Create(BsonDocument bd)
        {
            _userCollectionBson.InsertOne(bd);
        }

    }
}
