using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Service_Address.Models
{
    public class Regions
    {
        [BsonId]
        [BsonIgnoreIfDefault]
        [BsonRepresentation(BsonType.ObjectId)]
        public String? id { get; set; }

        public string? name { get; set; }

        public string? wikiDataId { get; set; }
    }

    public class RegionsFitler: Regions
    {
        public List<String>? ids { get; set; }
        public List<String>? names { get; set; }

    }

    public class Region: Regions
    {
        [BsonId]
        [BsonIgnoreIfDefault]
        [BsonRepresentation(BsonType.ObjectId)]
        public String? id { get; set; }

        public string? name { get; set; }

        public string? wikiDataId { get; set; }
    }
}
