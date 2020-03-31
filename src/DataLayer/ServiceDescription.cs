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
        public AzureServiceType Type;
        public AzureSupportLevel Supported;

        //TODO BSON ignore if null
        public BsonDocument Extras;
    }

    public class ServiceDescription : IObjectID
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string ServiceName;
        public ServiceProvider CloudProvider;
        public DateTime CreatedAt;
        public DateTime UpdatedAt;

        public BsonDocument Details;

        public static ServiceDescription CreateAzure(string Name, AzureServiceType type, AzureSupportLevel supportLevel, BsonDocument extra=null)
        {
            var serviceDesc = new ServiceDescription();
            serviceDesc.Id = new ObjectId();
            serviceDesc.CreatedAt = DateTime.Now;
            serviceDesc.UpdatedAt = DateTime.Now;
            serviceDesc.ServiceName = Name;
            serviceDesc.CloudProvider = ServiceProvider.Azure;

            AzureDetails details = new AzureDetails() { Supported = supportLevel, Type = type, Extras=extra };

            serviceDesc.Details = details.ToBsonDocument();
            return serviceDesc;
        }

        public override string ToString()
        {
            return $"{this.Id} {this.ServiceName} {this.CreatedAt.ToShortTimeString()}";
        }
    }
}
