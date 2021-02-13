namespace Coin
{
    using System;

    public class ConsoleChainExplorer
    {
        private readonly IBlockChain chain;

        public ConsoleChainExplorer(IBlockChain chain)
        {
            this.chain = chain;
        }

        public void PrintTransactionHistory(string name)
        {
            Console.WriteLine("\n\n====== Transaction History for {0} =====", name);

            foreach (var block in this.chain.Blocks)
            {
                var transactions = block.Transactions;
                foreach (var transaction in transactions)
                {
                    var sender = transaction.Sender;
                    var recipient = transaction.Recipient;

                    if (name.ToLower().Equals(sender.ToLower()) || name.ToLower().Equals(recipient.ToLower()))
                    {
                        Console.WriteLine("Timestamp :{0}", transaction.TimeStamp);
                        Console.WriteLine("Sender:   :{0}", transaction.Sender);
                        Console.WriteLine("Recipient :{0}", transaction.Recipient);
                        Console.WriteLine("Amount    :{0}", transaction.Amount);
                        Console.WriteLine("Fee       :{0}", transaction.Fee);
                        Console.WriteLine("--------------");
                    }
                }
            }
        }

        public void PrintBalance(string name)
        {
            Console.WriteLine("\n\n==== Balance for {0} ====", name);
            double balance = 0;
            double spending = 0;
            double income = 0;

            foreach (var block in this.chain.Blocks)
            {
                var transactions = block.Transactions;

                foreach (var transaction in transactions)
                {
                    var sender = transaction.Sender;
                    var recipient = transaction.Recipient;

                    if (name.ToLower().Equals(sender.ToLower()))
                    {
                        spending += transaction.Amount + transaction.Fee;
                    }

                    if (name.ToLower().Equals(recipient.ToLower()))
                    {
                        income += transaction.Amount;
                    }

                    balance = income - spending;
                }
            }

            Console.WriteLine("Balance :{0}", balance);
            Console.WriteLine("---------");
        }

        public void PrintLastBlock()
        {
            var lastBlock = this.chain.GetLastBlock;
            Console.WriteLine(lastBlock);
        }

        public void PrintGenesisBlock()
        {
            var block = this.chain.GetGenesisBlock;
            Console.WriteLine(block);
        }

        public void PrintBlocks()
        {
            Console.WriteLine("\n\n====== Blockchain Explorer =====");

            foreach (var block in this.chain.Blocks)
            {
                Console.WriteLine(block);
            }
        }
    }
}
