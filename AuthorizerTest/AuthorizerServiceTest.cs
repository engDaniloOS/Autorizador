using Authorizer.Business.Models;
using Authorizer.Services;
using Authorizer.Services.Infrastructure;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace AuthorizerTest
{
    public class AuthorizerServiceTest
    {
        private Mock<IAccountRepository> _accountRepository;
        private Mock<ITransactionRepository> _transactionRepository;

        [SetUp]
        public void Setup()
        {
            _accountRepository = new Mock<IAccountRepository>();
            _transactionRepository = new Mock<ITransactionRepository>();

            _accountRepository.Setup(x => x.ExistsAccount()).Returns(true);
        }

        #region Main Methods
        [Test]
        public void ExecuteTransactionSuccessfully()
        {
            var lastLimitAccount = 20;

            _accountRepository.Setup(x => x.GetAccount()).Returns(GetAccount());
            _accountRepository.Setup(x => x.UpdateAccountLimit(It.IsAny<int>())).Returns(GetAccount(lastLimitAccount));

            _transactionRepository.Setup(x => x.SaveNewTransaction(It.IsAny<AccountTransaction>())).Verifiable();
            _transactionRepository.Setup(x => x.GetTransactionsInInterval(It.IsAny<int>(), It.IsAny<DateTime>())).Returns(new List<AccountTransaction>());

            var transactionIn = GetTransactionIn(DateTime.Now);

            var service = new AuthorizerService(_transactionRepository.Object, _accountRepository.Object);

            var accountOut = service.AuthorizeAndExecuteTransaction(transactionIn);

            Assert.IsTrue(accountOut.Account.IsCardActive);
            Assert.AreEqual(lastLimitAccount, accountOut.Account.AvailableLimit);
            Assert.IsTrue(accountOut.Violations.Count == 0);
        }

        [Test]
        public void TryExecuteTransactionAndGetAccountNotInitializedViolation()
        {
            _accountRepository.Setup(x => x.ExistsAccount()).Returns(false);

            var transactionIn = GetTransactionIn(DateTime.Now);
            var service = new AuthorizerService(_transactionRepository.Object, _accountRepository.Object);

            var accountOut = service.AuthorizeAndExecuteTransaction(transactionIn);

            Assert.IsNull(accountOut.Account);
            Assert.IsTrue(accountOut.Violations.Count > 0);
            Assert.Contains(Violations.ACCOUNT_NOT_INITIALIZED, accountOut.Violations);
        }

        [TestCase]
        public void TryExecuteTransactionAndGetCardNotActiveViolation()
        {
            var accountValue = 50;
            var lastTransactions = new List<AccountTransaction>();

            _accountRepository.Setup(x => x.GetAccount()).Returns(GetAccount(accountValue, isActive: false));

            _transactionRepository.Setup(x => x.GetTransactionsInInterval(It.IsAny<int>(), It.IsAny<DateTime>())).Returns(lastTransactions); ;

            var transactionIn = GetTransactionIn(DateTime.Now, accountValue);
            var service = new AuthorizerService(_transactionRepository.Object, _accountRepository.Object);

            var accountOut = service.AuthorizeAndExecuteTransaction(transactionIn);

            Assert.AreEqual(accountValue, accountOut.Account.AvailableLimit);
            Assert.False(accountOut.Account.IsCardActive);
            Assert.IsTrue(accountOut.Violations.Count > 0);
            Assert.Contains(Violations.CARD_NOT_ACTIVE, accountOut.Violations);
        }

        [TestCase]
        public void TryExecuteTransactionAndGetInsufficientLimitViolation()
        {
            var accountValue = 50;
            var transactionAmount = 100;
            var lastTransactions = new List<AccountTransaction>();

            _accountRepository.Setup(x => x.GetAccount()).Returns(GetAccount(accountValue));

            _transactionRepository.Setup(x => x.GetTransactionsInInterval(It.IsAny<int>(), It.IsAny<DateTime>())).Returns(lastTransactions);

            var transactionIn = GetTransactionIn(DateTime.Now, transactionAmount);
            var service = new AuthorizerService(_transactionRepository.Object, _accountRepository.Object);

            var accountOut = service.AuthorizeAndExecuteTransaction(transactionIn);

            Assert.AreEqual(accountValue, accountOut.Account.AvailableLimit);
            Assert.IsTrue(accountOut.Violations.Count > 0);
            Assert.Contains(Violations.INSUFFICIENT_LIMIT, accountOut.Violations);
        }

        [TestCase]
        public void TryExecuteTransactionAndGetHighFrequencySmallIntervalViolation()
        {
            var accountValue = 100;
            var transactionAmount = 10;
            var limitTransactionsAllowed = 3;

            _accountRepository.Setup(x => x.GetAccount()).Returns(GetAccount(accountValue));

            _transactionRepository.Setup(x => x.GetTransactionsInInterval(It.IsAny<int>(), It.IsAny<DateTime>())).Returns(GetLastTransactions(limitTransactionsAllowed, transactionAmount, false));

            var transactionIn = GetTransactionIn(DateTime.Now, transactionAmount);
            var service = new AuthorizerService(_transactionRepository.Object, _accountRepository.Object);

            var accountOut = service.AuthorizeAndExecuteTransaction(transactionIn);

            Assert.AreEqual(accountValue, accountOut.Account.AvailableLimit);
            Assert.IsTrue(accountOut.Violations.Count > 0);
            Assert.Contains(Violations.HIGH_FREQUENCY_SMALL_INTERVAL, accountOut.Violations);
        }

        [TestCase]
        public void TryExecuteTransactionAndGetDoubledTransactionViolation()
        {
            var accountValue = 100;
            var transactionAmount = 100;
            var limitTransactionsAllowed = 3;

            _accountRepository.Setup(x => x.GetAccount()).Returns(GetAccount(accountValue));

            _transactionRepository.Setup(x => x.GetTransactionsInInterval(It.IsAny<int>(), It.IsAny<DateTime>())).Returns(GetLastTransactions(limitTransactionsAllowed, transactionAmount, true));

            var transactionIn = GetTransactionIn(DateTime.Now, transactionAmount);
            var service = new AuthorizerService(_transactionRepository.Object, _accountRepository.Object);

            var accountOut = service.AuthorizeAndExecuteTransaction(transactionIn);

            Assert.AreEqual(accountValue, accountOut.Account.AvailableLimit);
            Assert.IsTrue(accountOut.Violations.Count > 0);
            Assert.Contains(Violations.DOUBLED_TRANSACTION, accountOut.Violations);
        }

        [TestCase]
        public void TryExecuteTransactionAndGetHighFrequencySmallIntervalAndDoubledTransactionAndInsufficientLimitAndCardNotActiveViolations()
        {
            var accountValue = 50;
            var transactionAmount = 100;
            var limitTransactionsAllowed = 3;

            _accountRepository.Setup(x => x.GetAccount()).Returns(GetAccount(accountValue, false));

            _transactionRepository.Setup(x => x.GetTransactionsInInterval(It.IsAny<int>(), It.IsAny<DateTime>())).Returns(GetLastTransactions(limitTransactionsAllowed, transactionAmount, true));

            var transactionIn = GetTransactionIn(DateTime.Now, transactionAmount);
            var service = new AuthorizerService(_transactionRepository.Object, _accountRepository.Object);

            var accountOut = service.AuthorizeAndExecuteTransaction(transactionIn);

            Assert.AreEqual(accountValue, accountOut.Account.AvailableLimit);
            Assert.IsTrue(accountOut.Violations.Count > 0);
            Assert.Contains(Violations.DOUBLED_TRANSACTION, accountOut.Violations);
            Assert.Contains(Violations.HIGH_FREQUENCY_SMALL_INTERVAL, accountOut.Violations);
            Assert.Contains(Violations.INSUFFICIENT_LIMIT, accountOut.Violations);
            Assert.Contains(Violations.CARD_NOT_ACTIVE, accountOut.Violations);
        } 
        #endregion

        #region Assistant methods
        private AccountTransactionIn GetTransactionIn(DateTime time, int amount = 10, string merchant = "xpto")
        {
            var transaction = new AccountTransaction
            {
                Amount = amount,
                Merchant = merchant,
                Time = DateTime.Now
            };

            return new AccountTransactionIn { Transaction = transaction };
        }

        private Account GetAccount(int limit = 100, bool isActive = true) => new Account { AvailableLimit = limit, IsCardActive = isActive };

        private List<AccountTransaction> GetLastTransactions(int quantity, int amount = 100, bool sameAmount = true)
        {
            var lastTransactions = new List<AccountTransaction>();

            for (int i = 1; i <= quantity; i++)
                lastTransactions.Add(
                    new AccountTransaction
                    {
                        Amount = sameAmount ? amount : amount * i,
                        Merchant = "xpto",
                        Time = DateTime.Now
                    });

            return lastTransactions;
        } 
        #endregion
    }
}