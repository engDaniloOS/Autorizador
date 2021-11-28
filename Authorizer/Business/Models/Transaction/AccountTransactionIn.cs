using System.Text.Json.Serialization;

namespace Authorizer.Business.Models
{
    public class AccountTransactionIn
    {
        [JsonPropertyName("transaction")]
        public AccountTransaction Transaction { get; set; }
    }
}
