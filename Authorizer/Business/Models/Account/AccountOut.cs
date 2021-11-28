using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Authorizer.Business.Models
{
    public class AccountOut
    {
        [JsonPropertyName("account")]
        public Account Account { get; set; }

        [JsonPropertyName("violations")]
        public List<string> Violations { get; set; }
    }
}
