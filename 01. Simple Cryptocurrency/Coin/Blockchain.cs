namespace Coin
{
    using System.Collections.Generic;
    using System.Linq;

    // Blockchain is a transaction record shared and stored on a distributed computer, unchanged and open to the public.
    // Blockchain stores transactions by grouping them into groups called a block.
    public class BlockChain : IBlockChain
    {
        private readonly IList<Block> blocks;
        private readonly ITransactionPool transactionPool;

        public BlockChain(ITransactionPool transactionPool)
        {
            this.blocks = new List<Block> { CreateGenesisBlock() };
            this.transactionPool = transactionPool;
        }

        public IEnumerable<Block> Blocks => this.blocks.ToList().AsReadOnly();

        public Block GetLastBlock => this.blocks.Last();

        public Block GetGenesisBlock => this.blocks.First();

        public ITransactionPool TransactionsPool => this.transactionPool;

        // Adds new block to the chain and clears transaction pool
        public void AddBlock()
        {
            var lastBlock = this.GetLastBlock;
            var nextHeight = lastBlock.Height + 1;
            var prevHash = lastBlock.Hash;
            var block = new Block(nextHeight, prevHash, this.transactionPool.Transactions, "Admin");

            this.blocks.Add(block);
            this.transactionPool.ClearPool();
        }

        private static Block CreateGenesisBlock()
        {
            var transactions = new List<Transaction>();
            var transaction = new Transaction(sender: "System", recipient: "Genesis Account", amount: 1000, 0.0001);
            transactions.Add(transaction);

            return new Block(height: 1, previousHash: string.Empty.ConvertToBytes(), transactions: transactions.ToArray(), creator: "Admin");
        }
    }
}
