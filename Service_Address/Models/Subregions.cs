using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Service_Address.Models
{
    public class Subregions
    {
        [BsonId]
        [BsonIgnoreIfDefault]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }
        public string? name { get; set; }
        public string? region_id { get; set; }

        public string? wikiDataId { get; set; }
    }

    public class SubregionsFitler : Subregions
    {
        public List<string>? ids { get; set; }
        public List<string>? names { get; set; }
        public List<string>? region_ids { get; set; }

        public List<string>? wikiDataIds { get; set;}
    }
}
