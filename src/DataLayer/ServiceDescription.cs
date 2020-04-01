using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace DataLayer
{
    public enum ServiceProvider { Azure, AWS }
    public enum AzureServiceType { Compute, Storage }
    public enum AzureSupportLevel { Current, Classic, Deprecated }

    public class AzureDetails
    {
        public AzureServiceType Type { get; set; }
        public AzureSupportLevel Supported { get; set; }

        //TODO BSON ignore if null
        //public BsonDocument Extras { get; set; }
    }

    public class ServiceDescription : IObjectID
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } //If you leave this blank one will be assigned by Cosmos

        public string ServiceName { get; set; }
        public ServiceProvider CloudProvider { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public AzureDetails Details { get; set; }

        public static ServiceDescription CreateAzure(string Name, AzureServiceType type, AzureSupportLevel supportLevel, BsonDocument extra=null)
        {
            var serviceDesc = new ServiceDescription();
            serviceDesc.CreatedAt = DateTime.Now;
            serviceDesc.UpdatedAt = DateTime.Now;
            serviceDesc.ServiceName = Name;
            serviceDesc.CloudProvider = ServiceProvider.Azure;

            AzureDetails details = new AzureDetails() { Supported = supportLevel, Type = type};

            serviceDesc.Details = details;
            return serviceDesc;
        }

        public override string ToString()
        {
            return $"{this.Id} {this.ServiceName} {this.CreatedAt.ToShortTimeString()}";
        }
    }
}
