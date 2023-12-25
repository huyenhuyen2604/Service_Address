using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Service_Address.Models
{
    public class Subregions
    {
        [BsonId]
        [BsonIgnoreIfDefault]
        [BsonRepresentation(BsonType.ObjectId)]
        public int id { get; set; }
        public string? name { get; set; }
        public string? region_id { get; set; }

        public string? wikiDataId { get; set; }
    }
}
