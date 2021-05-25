namespace Client
{
    using System;

    using Coin;

    public static class Program
    {
        public static void Main()
        {
            // Demo();

            // Make blockchain
            var bc = new BlockChain();

            var engine = new Engine(bc);
            engine.Run();
        }

        private static void Demo()
        {
            // Initialize blockchain
            var transactionPool = new TransactionPool();
            var blockChain = new BlockChain(transactionPool);

            // Create new transactions.
            var transaction1 = new Transaction(sender: "Bob", recipient: "Billy", amount: 10, fee: 0.01);
            var transaction2 = new Transaction(sender: "John", recipient: "Ivanka", amount: 20, fee: 0.01);
            var transaction3 = new Transaction(sender: "Robert", recipient: "Antonio", amount: 30, fee: 0.01);

            // Add transactions to transactions pool
            blockChain.TransactionsPool.AddTransaction(transaction1);
            blockChain.TransactionsPool.AddTransaction(transaction2);
            blockChain.TransactionsPool.AddTransaction(transaction3);

            // Forging a block - BlockChain will add a new block to the chain and add transactions to it
            blockChain.AddBlock();

            // Create another transactions.
            transaction1 = new Transaction(sender: "Asen", recipient: "Ivan", amount: 110, fee: 0.01);
            transaction2 = new Transaction(sender: "Gosho", recipient: "Boqn", amount: 220, fee: 0.01);
            transaction3 = new Transaction(sender: "Pesho", recipient: "Martin", amount: 330, fee: 0.01);

            // Add transactions to transactions pool
            blockChain.TransactionsPool.AddTransaction(transaction1);
            blockChain.TransactionsPool.AddTransaction(transaction2);
            blockChain.TransactionsPool.AddTransaction(transaction3);

            // Forging another block
            blockChain.AddBlock();

            // We have 2 blocks and 6 transactions
            // Chain explorer will output data from block on console
            var chainExplorer = new ConsoleChainExplorer(blockChain);

            chainExplorer.PrintGenesisBlock();
            chainExplorer.PrintLastBlock();
            chainExplorer.PrintBalance("Robert");
            chainExplorer.PrintBalance("Antonio");
            chainExplorer.PrintBalance("Martin");

            chainExplorer.PrintTransactionHistory("Robert");
            chainExplorer.PrintTransactionHistory("Gosho");
            chainExplorer.PrintTransactionHistory("Boqn");

            Console.WriteLine();
            chainExplorer.PrintBlocks();

            Console.WriteLine("Press enter to close application");
            Console.ReadLine();
        }
    }
}
