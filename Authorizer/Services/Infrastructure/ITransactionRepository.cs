using Authorizer.Business.Models;
using System;
using System.Collections.Generic;

namespace Authorizer.Services.Infrastructure
{
    public interface ITransactionRepository
    {
        void SaveNewTransaction(AccountTransaction transaction);
        List<AccountTransaction> GetTransactionsInInterval(int minutes, DateTime transactionTime);
    }
}
