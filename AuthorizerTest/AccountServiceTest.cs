using Authorizer.Business.Models;
using Authorizer.Services;
using Authorizer.Services.Infrastructure;
using Moq;
using NUnit.Framework;

namespace AuthorizerTest
{
    public class AccountServiceTest
    {
        private Mock<IAccountRepository> _accountRepository;

        public int AvailableLimit { get; } = 100;
        public bool IsCardActive { get; } = true;

        [SetUp]
        public void Setup() => _accountRepository = new Mock<IAccountRepository>();

        #region Main methods
        [Test]
        public void CreateNewAccountSuccessfully()
        {
            _accountRepository.Setup(x => x.ExistsAccount()).Returns(false);
            _accountRepository.Setup(x => x.SaveNewAccount(It.IsAny<Account>())).Returns(GetAccount());

            var service = new AccountService(_accountRepository.Object);
            var accountOut = service.CreateNew(GetAccountIn());

            Assert.AreEqual(IsCardActive, accountOut.Account.IsCardActive);
            Assert.AreEqual(AvailableLimit, accountOut.Account.AvailableLimit);
            Assert.IsTrue(accountOut.Violations.Count == 0);
        }

        [Test]
        public void TryCreateNewAccountAndGetAccountAlreadyInitializedViolation()
        {
            _accountRepository.Setup(x => x.ExistsAccount()).Returns(true);
            _accountRepository.Setup(x => x.GetAccount()).Returns(GetAccount());

            var service = new AccountService(_accountRepository.Object);
            var accountOut = service.CreateNew(GetAccountIn());

            Assert.AreEqual(IsCardActive, accountOut.Account.IsCardActive);
            Assert.AreEqual(AvailableLimit, accountOut.Account.AvailableLimit);
            Assert.IsTrue(accountOut.Violations.Count > 0);
            Assert.Contains(Violations.ACCOUNT_ALREADY_INITIALIZED, accountOut.Violations);
        } 
        #endregion

        #region Assistant Methods
        private Account GetAccount() =>
    new Account { AvailableLimit = AvailableLimit, IsCardActive = IsCardActive };

        private AccountIn GetAccountIn() =>
            new AccountIn { Account = new Account { AvailableLimit = AvailableLimit, IsCardActive = IsCardActive } }; 
        #endregion
    }
}
