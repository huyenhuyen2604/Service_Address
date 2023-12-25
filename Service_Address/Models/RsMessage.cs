using System.Runtime.Serialization;

namespace Service_Address.Models
{
    public class RsMessage
    {
        
        public bool? status { get; set; }

        
        public string? message { get; set; }

        
        public object? data { get; set; }
    }
}
