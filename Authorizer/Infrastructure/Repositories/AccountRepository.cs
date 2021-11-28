using Authorizer.Business.Models;
using Authorizer.Services.Infrastructure;

namespace Authorizer.Infrastructure
{
    public class AccountRepository : IAccountRepository
    {
        public Account Account { get; private set; }

        public bool ExistsAccount() => Account != null;

        public Account GetAccount()
        {
            return new Account
            {
                AvailableLimit = Account.AvailableLimit,
                IsCardActive = Account.IsCardActive
            };
        }

        public Account SaveNewAccount(Account newAccount)
        {
            Account = newAccount;

            return newAccount;
        }

        public Account UpdateAccountLimit(int amount)
        {
            Account.AvailableLimit -= amount;

            return new Account
            {
                AvailableLimit = Account.AvailableLimit,
                IsCardActive = Account.IsCardActive
            };
        }
    }
}
