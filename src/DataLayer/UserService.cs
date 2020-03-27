using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Security.Authentication;

namespace DataLayer
{
    public class UserService
    {
        readonly IMongoCollection<User> _userCollection;

        public UserService(string connectionString)
        {
            MongoClientSettings settings = MongoClientSettings.FromUrl(
              new MongoUrl(connectionString)
            );
            settings.SslSettings =
              new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            MongoClient client = new MongoClient(settings);

            var db = client.GetDatabase("People");
            _userCollection = db.GetCollection<User>("UserCollection");
        }

        public List<User> List() => _userCollection.Find(p => true).ToList();

        public void Insert(User u) => _userCollection.InsertOne(u);

        public User Get(ObjectId id) => _userCollection.Find<User>(u => u.Id == id).FirstOrDefault();

        public void Delete(User user) => _userCollection.DeleteOne<User>(u => u.Id == user.Id);
    }
}