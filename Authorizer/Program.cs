using Authorizer.Business.Models;
using Authorizer.Business.Services;
using Authorizer.Infrastructure;
using Authorizer.Services;
using Authorizer.Services.Infrastructure;
using System;
using System.Text.Json;

namespace Authorizer
{
    public class Program
    {
        private static IAccountService _accountService;
        private static IAccountRepository _accountRepository;

        private static IAuthorizerService _authorizerService;
        private static ITransactionRepository _transactionRepository;

        public static void Main(string[] args)
        {
            _accountRepository = new AccountRepository();
            _accountService = new AccountService(_accountRepository);

            _transactionRepository = new TransactionRepository();
            _authorizerService = new AuthorizerService(_transactionRepository, _accountRepository);

            while (true)
                try
                {
                    var operation = Console.ReadLine();

                    if (IsOperationANewAccount(operation))
                        RunNewAccount(operation);
                    else
                        RunNewTransaction(operation);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
        }

        private static void RunNewAccount(string operation)
        {
            var newAccountIn = JsonSerializer.Deserialize<AccountIn>(operation);

            var accountOut = _accountService.CreateNew(newAccountIn);

            Console.WriteLine(JsonSerializer.Serialize(accountOut));
        }

        private static void RunNewTransaction(string operation)
        {
            var newTransactionIn = JsonSerializer.Deserialize<AccountTransactionIn>(operation);

            var accountOut = _authorizerService.AuthorizeAndExecuteTransaction(newTransactionIn);

            if (accountOut.Account == null)
            {
                var accountNotInitialized = new { account = new object(), violations = accountOut.Violations };

                Console.WriteLine(JsonSerializer.Serialize(accountNotInitialized));
            }
            else
                Console.WriteLine(JsonSerializer.Serialize(accountOut));
        }

        private static bool IsOperationANewAccount(string operation)
        {
            try
            {
                var newAccount = JsonSerializer.Deserialize<AccountIn>(operation);

                return (newAccount.Account != null);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
