﻿namespace Coin
{
    using System.Collections.Generic;
    using System.Linq;

    using Coin.Utils;

    // Blockchain is a transaction record shared and stored on a distributed computer, unchanged and open to the public.
    // Blockchain stores transactions by grouping them into groups called a block.
    public class BlockChain : IBlockChain
    {
        private readonly IList<Block> blocks;
        private readonly ITransactionPool transactionPool;

        public BlockChain()
            : this(new TransactionPool())
        {
        }

        public BlockChain(ITransactionPool transactionPool)
        {
            this.blocks = new List<Block> { CreateGenesisBlock() };
            this.transactionPool = transactionPool;
        }

        public int Count => this.blocks.Count;

        public Block GetLastBlock => this.blocks.Last();

        public Block GetGenesisBlock => this.blocks.First();

        public ITransactionPool TransactionsPool => this.transactionPool;

        public IEnumerable<Block> Blocks => this.blocks.ToList().AsReadOnly();

        public void AddTransaction(Transaction transaction)
        {
            this.transactionPool.AddTransaction(transaction);
        }

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

        public double GetBalance(string name)
        {
            double balance = 0;
            double spending = 0;
            double income = 0;

            foreach (var block in this.Blocks)
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

            return balance;
        }

        public IEnumerable<Block> GetBlocks(int pageNumber, int resultPerPage)
            => this.blocks
                .OrderByDescending(x => x.Height)
                .Skip((pageNumber - 1) * resultPerPage)
                .Take(resultPerPage)
                .ToList();

        public IEnumerable<Transaction> GetTransactions(string address)
            => this.Blocks
                .SelectMany(b => b.Transactions)
                .Where(t => t.Sender == address || t.Recipient == address)
                .ToArray();

        private static Block CreateGenesisBlock()
        {
            var transactions = new List<Transaction>();

            foreach (var account in Ico.Accounts)
            {
                var transaction = new Transaction("ico", account.Address, account.Balance, 0.0F);

                transaction.Hash = TransactionHelpers.GetTransactionHash(transaction);

                transactions.Add(transaction);
            }

            return new Block(
                height: 0,
                previousHash: string.Empty,
                transactions: transactions.ToArray(),
                validator: "Admin");
        }
    }
}
