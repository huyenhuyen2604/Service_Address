namespace Service_Address.Models
{
    public class Cities
    {
        public string? id { get; set; }


        public string? name { get; set; }


        public int? state_id { get; set; }


        public string? state_code { get; set; }


        public string? state_name { get; set; }


        public int? country_id { get; set; }


        public string? country_code { get; set; }


        public string? country_name { get; set; }


        public string? latitude { get; set; }


        public string? longitude { get; set; }


        public string? wikiDataId { get; set; }
    }

    public class CitiesFilter : Cities
    {
        public List<String>? ids { get; set; }
        public List<String>? names { get; set; }
        public List<String>? state_codes { get; set; }
        public List<String>? state_names { get; set; }
        public List<String>? country_codes { get; set; }
        public List<String>? country_names { get; set; }

    }
}
