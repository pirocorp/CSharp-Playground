namespace Coin
{
    using System.Collections.Generic;
    using System.Linq;

    public class TransactionPool : ITransactionPool
    {
        private ICollection<Transaction> transactions;

        public TransactionPool()
        {
            this.transactions = new List<Transaction>();
        }

        public IEnumerable<Transaction> Transactions => this.transactions.ToList().AsReadOnly();

        public void AddTransaction(Transaction transaction) => this.transactions.Add(transaction);

        public void ClearPool() => this.transactions = new List<Transaction>();
    }
}
