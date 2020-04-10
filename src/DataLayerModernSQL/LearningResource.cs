using System;

namespace DataLayerModernSQL
{
    public class LearningResource
    {
        public Guid Id { get; set; } //If you leave this blank one will be assigned by Cosmos

        public Guid ServiceID { get; set; }

        public string Name { get; set; }

        public Uri Uri { get; set; }

        //TODO add tags
        //TODO add description
    }
}
