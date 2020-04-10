using System;
using System.Collections.Generic;

namespace DataLayerModernSQL
{
    public enum ServiceProvider { Azure, AWS }
    public enum AzureServiceType { Compute, Storage, Database }
    public enum AzureSupportLevel { Current, Classic, Deprecated, Preview }

    public class AzureDetails
    {
        public Guid Id { get; set; }
        public AzureServiceType Type { get; set; }
        public AzureSupportLevel Supported { get; set; }
    }

    public class ServiceDescription
    {
        public Guid Id { get; set; }

        public string ServiceName { get; set; }
        public ServiceProvider CloudProvider { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public AzureDetails Details { get; set; }

        public List<LearningResource> LearningResources { get; set; }

        public static ServiceDescription CreateAzure(string Name, AzureServiceType type, AzureSupportLevel supportLevel)
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
