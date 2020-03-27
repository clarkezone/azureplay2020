using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Diagnostics;
using System.Security.Authentication;


public class User
{
    public MongoDB.Bson.BsonObjectId ID;
    public string Name;
    public string Address;
    public DateTime CreatedAt;
    public DateTime UpdatedAt;

    public static User Create(string Name, string Address)
    {
        var user = new User();
        user.CreatedAt = DateTime.Now;
        user.UpdatedAt = DateTime.Now;
        user.Name = Name;
        return user;
    }
}

public class DAL
{
    MongoClient _client;

    public void Init()
    {


        string connectionString =
  @"mongodb://clarkezonetestcosmosforplay:QV6W2fWmedSoEGvQSVAzP3IOUZWtTyjIkGq2k4Yxna6JEm0lvGm4p7zbcPOMaKYwMzkWCxJTguDw5QYbPh4cng==@clarkezonetestcosmosforplay.mongo.cosmos.azure.com:10255/?ssl=true&replicaSet=globaldb&maxIdleTimeMS=120000&appName=@clarkezonetestcosmosforplay@";
        MongoClientSettings settings = MongoClientSettings.FromUrl(
          new MongoUrl(connectionString)
        );
        settings.SslSettings =
          new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
        _client = new MongoClient(settings);
    }

    public void GetPeople()
    {
        var result = _client.ListDatabases();

    }

    public async void CreatePerson(User u)
    {
        var db = _client.GetDatabase("People");
        var collection = db.GetCollection<BsonDocument>("People Collection");
        try
        {
            await collection.InsertOneAsync(u.ToBsonDocument());
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }

    }

}