using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer
{
    public enum AzureServiceType { Compute, Storage}

    [Serializable]
    public class AzureServiceDescription : IObjectID
    {
        private AzureServiceDescription()
        {

        }

        [BsonId]
        public ObjectId Id { get; set; }

        public string ServiceName;
        public AzureServiceType Type;
        public DateTime CreatedAt;
        public DateTime UpdatedAt;

        public static AzureServiceDescription Create(string Name, AzureServiceType type)
        {
            var serviceDesc = new AzureServiceDescription();
            serviceDesc.Id = new ObjectId();
            serviceDesc.CreatedAt = DateTime.Now;
            serviceDesc.UpdatedAt = DateTime.Now;
            serviceDesc.ServiceName = Name;
            serviceDesc.Type = type;
            return serviceDesc;
        }

        public override string ToString()
        {
            return $"{this.Id} {this.ServiceName} {this.Type} {this.CreatedAt.ToShortTimeString()}";
        }
    }
}
