using Authorizer.Business.Models;

namespace Authorizer.Business.Services
{
    public interface IAuthorizerService
    {
        AccountOut AuthorizeAndExecuteTransaction(AccountTransactionIn transactionIn);
    }
}
