using System.Text.Json.Serialization;

namespace Authorizer.Business.Models
{
    public class Account
    {
        [JsonPropertyName("active-card")]
        public bool IsCardActive { get; set; }

        [JsonPropertyName("available-limit")]
        public int AvailableLimit { get; set; }
    }
}
