namespace Coin
{
    using System.Collections.Generic;

    public interface IBlockChain
    {
        IEnumerable<Block> Blocks { get; }

        ITransactionPool TransactionsPool { get; }

        Block GetLastBlock { get;  }

        Block GetGenesisBlock { get; }

        void AddBlock();
    }
}
