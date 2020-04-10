using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace DataLayerMongo
{
    public interface IObjectID
    {
        string Id { get; }
    }

    [Serializable]
    public class User : IObjectID
    {
        private User()
        {

        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name;
        public string Address;
        public DateTime CreatedAt;
        public DateTime UpdatedAt;

        public static User Create(string Name, string Address)
        {
            var user = new User();
            user.Id = new ObjectId().ToString();
            user.CreatedAt = DateTime.Now;
            user.UpdatedAt = DateTime.Now;
            user.Name = Name;
            user.Address = Address;
            return user;
        }

        public override string ToString()
        {
            return $"{this.Id} {this.Name} {this.Address} {this.CreatedAt.ToShortTimeString()}";
        }
    }
}
