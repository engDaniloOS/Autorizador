using System;
using System.Text.Json.Serialization;

namespace Authorizer.Business.Models
{
    public class AccountTransaction
    {
        [JsonPropertyName("merchant")]
        public string Merchant { get; set; }

        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        [JsonPropertyName("time")]
        public DateTime Time { get; set; }
    }
}
