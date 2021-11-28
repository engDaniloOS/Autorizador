namespace Authorizer.Business.Models
{
    public static class Violations
    {
        public static string ACCOUNT_NOT_INITIALIZED { get; } = "account-not-initialized";
        public static string CARD_NOT_ACTIVE { get; } = "card-not-active";
        public static string INSUFFICIENT_LIMIT { get; } = "insufficient-limit";
        public static string HIGH_FREQUENCY_SMALL_INTERVAL { get; } = "high-frequency-small-interval";
        public static string DOUBLED_TRANSACTION { get; } = "doubled-transaction";
        public static string ACCOUNT_ALREADY_INITIALIZED { get; } = "account-already-initialized";
    }
}
