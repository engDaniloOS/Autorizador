using Authorizer.Business.Models;
using Authorizer.Business.Services;
using Authorizer.Services.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace Authorizer.Services
{
    public class AuthorizerService : IAuthorizerService
    {
        public readonly ITransactionRepository _transactionRepository;
        public readonly IAccountRepository _accountRepository;

        public AuthorizerService(ITransactionRepository transactionRepository, IAccountRepository accountRepository)
        {
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
        }

        public AccountOut AuthorizeAndExecuteTransaction(AccountTransactionIn transactionIn)
        {
            if (!_accountRepository.ExistsAccount())
                return new AccountOut
                {
                    Account = null,
                    Violations = new List<string> { Violations.ACCOUNT_NOT_INITIALIZED }
                };

            var violations = GetViolations(transactionIn.Transaction);

            if (violations.Count > 0)
                return new AccountOut { Account = _accountRepository.GetAccount(), Violations = violations };

            _transactionRepository.SaveNewTransaction(transactionIn.Transaction);

            var updatedAccount = _accountRepository.UpdateAccountLimit(transactionIn.Transaction.Amount);

            return new AccountOut { Account = updatedAccount, Violations = new List<string>() };
        }

        private List<string> GetViolations(AccountTransaction transaction)
        {
            var violations = new List<string>();
            var limitTransactionInPeriod = 2;
            var intervalMinBetweenTransactions = 2;

            var savedAccount = _accountRepository.GetAccount();
            if (!savedAccount.IsCardActive)
                violations.Add(Violations.CARD_NOT_ACTIVE);

            var isLimitInsufficient = savedAccount.AvailableLimit < transaction.Amount;
            if (isLimitInsufficient)
                violations.Add(Violations.INSUFFICIENT_LIMIT);

            var lastTransatcions = _transactionRepository.GetTransactionsInInterval(intervalMinBetweenTransactions, transaction.Time);
            if (lastTransatcions.Count > limitTransactionInPeriod)
                violations.Add(Violations.HIGH_FREQUENCY_SMALL_INTERVAL);

            var isThereSimilarTransactions =
                lastTransatcions.Where(t => t.Merchant == transaction.Merchant && t.Amount == transaction.Amount).ToList().Count() > 0;
            if (isThereSimilarTransactions)
                violations.Add(Violations.DOUBLED_TRANSACTION);

            return violations;
        }
    }
}
