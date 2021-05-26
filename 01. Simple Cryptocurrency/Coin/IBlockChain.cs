namespace Coin
{
    using System.Collections.Generic;

    public interface IBlockChain
    {
        int Count { get; }

        Block GetLastBlock { get; }

        Block GetGenesisBlock { get; }

        IEnumerable<Block> Blocks { get; }

        ITransactionPool TransactionsPool { get; }

        void AddBlock();

        void AddTransaction(Transaction transaction);

        double GetBalance(string name);

        IEnumerable<Block> GetBlocks(int pageNumber, int resultPerPage);

        IEnumerable<Transaction> GetTransactions(string address);
    }
}
