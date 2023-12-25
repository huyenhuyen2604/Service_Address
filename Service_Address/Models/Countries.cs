using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Service_Address.Models
{
    public class Countries
    {
        [BsonId]
        [BsonIgnoreIfDefault]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }

        public string? name { get; set; }

        public string? iso3 { get; set; }

        public string? iso2 { get; set; }

        public string? numeric_code { get; set; }

        public string? phone_code { get; set; }

        public string? capital { get; set; }

        public string? region { get; set; }

        public string? region_id { get; set; }

        public string? subregion { get; set; }

        public string? subregion_id { get; set; }

        public string? nationality { get; set; }

       
    }


    



    public class CountriesFitler : Countries
    {
        public List<string>? ids { get; set; }

        public List<string>? names { get; set; }

        public List<string>? isos3 { get; set; }

        public List<string>? isos2 { get; set; }

        public List<string>? numeric_codes { get; set; }

        public List<string>? phone_codes { get; set; }

        public List<string>? capitals { get; set; }

        public List<string>? regions { get; set; }

        public List<string>? region_ids { get; set; }

        public List<string>? subregions { get; set; }

        public List<string>? subregion_ids { get; set; }

        public List<string>? nationalitys { get; set; }

        
    }
}
