using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace DataLayerMongo
{
    public class LearningResource : IObjectID
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } //If you leave this blank one will be assigned by Cosmos

        [BsonRepresentation(BsonType.ObjectId)]
        public string ServiceID { get; set; }

        public string Name { get; set; }

        public Uri Uri { get; set; }

        //TODO add tags
        //TODO add description
    }
}
