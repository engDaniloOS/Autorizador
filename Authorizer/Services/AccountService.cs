using Authorizer.Business.Models;
using Authorizer.Business.Services;
using Authorizer.Services.Infrastructure;
using System.Collections.Generic;

namespace Authorizer.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repository;

        public AccountService(IAccountRepository repository) => _repository = repository;

        public AccountOut CreateNew(AccountIn newAccountIn)
        {
            if (_repository.ExistsAccount())
                return new AccountOut
                {
                    Account = _repository.GetAccount(),
                    Violations = new List<string> { Violations.ACCOUNT_ALREADY_INITIALIZED }
                };

            return new AccountOut
            {
                Account = _repository.SaveNewAccount(newAccountIn.Account),
                Violations = new List<string>()
            };
        }
    }
}
