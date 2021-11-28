using Authorizer.Business.Models;
using Authorizer.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Authorizer.Infrastructure
{
    public class TransactionRepository: ITransactionRepository
    {
        public List<AccountTransaction> Transactions { get ; set; }

        public List<AccountTransaction> GetTransactionsInInterval(int minutes, DateTime transactionTime)
        {
            if (Transactions == null)
                return new List<AccountTransaction>();

            return Transactions
                    .Where(transaction => transaction.Time >= transactionTime.AddMinutes(-minutes))
                    .ToList();
        }

        public void SaveNewTransaction(AccountTransaction transaction) 
        {
            if (Transactions == null)
                Transactions = new List<AccountTransaction>();

            Transactions.Add(transaction);
        }
    }
}
