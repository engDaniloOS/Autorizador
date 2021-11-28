using Authorizer.Business.Models;

namespace Authorizer.Services.Infrastructure
{
    public interface IAccountRepository
    {
        bool ExistsAccount();
        Account GetAccount();
        Account SaveNewAccount(Account newAccount);
        Account UpdateAccountLimit(int amount);
    }
}
