using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Service_Address.Models
{
    public class States
    {
        [BsonId]
        [BsonIgnoreIfDefault]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }


        public string? name { get; set; }


        public int? country_id { get; set; }


        public string? country_code { get; set; }


        public string? country_name { get; set; }


        public string? state_code { get; set; }


        public object? type { get; set; }


        public string? latitude { get; set; }


        public string? longitude { get; set; }


    }

    public class StatesFilter : States
    {
        public List<string>? ids { get; set; }

        public List<string>? names { get; set; }

        public List<string>? country_codes { get; set; }

        public List<string>? country_names { get; set; }


    }
}
