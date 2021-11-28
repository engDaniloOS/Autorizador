using System.Text.Json.Serialization;

namespace Authorizer.Business.Models
{
    public class AccountIn
    {
        [JsonPropertyName("account")]
        public Account Account { get; set; }
    }
}
