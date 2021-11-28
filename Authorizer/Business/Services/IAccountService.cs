using Authorizer.Business.Models;

namespace Authorizer.Business.Services
{
    public interface IAccountService
    {
        AccountOut CreateNew(AccountIn newAccount);
    }
}
