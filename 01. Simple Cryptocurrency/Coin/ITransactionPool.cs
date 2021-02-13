namespace Coin
{
    using System.Collections.Generic;

    public interface ITransactionPool
    {
        IEnumerable<Transaction> Transactions { get; }

        void AddTransaction(Transaction transaction);

        void ClearPool();
    }
}
