namespace Client
{
    using System;
    using System.Globalization;
    using System.Linq;

    using Coin;
    using Coin.Utils;
    using Newtonsoft.Json;

    public class Engine
    {
        private readonly BlockChain bc;

        public Engine(BlockChain blockchain)
        {
            this.bc = blockchain;
        }

        public void Run()
        {
            this.SeedData();
            this.MenuScreen();
            this.GetInputFromUser();
        }

        private void SeedData()
        {
            var ga1 = new Transaction("System", "ga1", 10000, 0.001);
            var ga2 = new Transaction("System", "ga2", 5000, 0.002);

            this.bc.TransactionsPool.AddTransaction(ga1);
            this.bc.TransactionsPool.AddTransaction(ga2);

            this.bc.AddBlock();
        }

        private void MenuScreen()
        {
            Console.WriteLine("=========================");
            Console.WriteLine("1. Get Genesis Block");
            Console.WriteLine("2. Get Last Block");
            Console.WriteLine("3. Send Money");
            Console.WriteLine("4. Create Block (mining)");
            Console.WriteLine("5. Get Balance");
            Console.WriteLine("6. Transaction History");
            Console.WriteLine("7. Show Blockchain");
            Console.WriteLine("8. Exit");
            Console.WriteLine("=========================");
        }

        private void GetInputFromUser()
        {
            var selection = 0;

            while (selection != 20)
            {
                switch (selection)
                {
                    case 1:
                        this.DoGenesisBlock();
                        break;
                    case 2:
                        this.DoLastBlock();
                        break;
                    case 3:
                        this.DoSendCoin();
                        break;
                    case 4:
                        this.DoCreateBlock();
                        break;
                    case 5:
                        this.DoGetBalance();
                        break;
                    case 6:
                        this.DoGetTransactionHistory();
                        break;
                    case 7:
                        this.DoShowBlockchain();
                        break;
                    case 8:
                        this.DoExit();
                        break;
                }

                if (selection != 0)
                {
                    Console.WriteLine("\n===== Press enter to continue! =====");
                    var strKey = Console.ReadLine();

                    if (strKey != null)
                    {
                        Console.Clear();
                        this.MenuScreen();
                    }
                }

                Console.WriteLine("\n**** Please select menu!!! *****");
                var action = Console.ReadLine();

                var success = int.TryParse(action, out selection);

                if (!success)
                {
                    Console.Clear();
                    this.MenuScreen();
                }
            }
        }

        private void DoGenesisBlock()
        {
            Console.Clear();
            Console.WriteLine("\n\n\n\nGenesis Block");
            Console.WriteLine("Time: {0}", DateTime.Now);
            Console.WriteLine("======================");
            var genesisBlock = this.bc.GetGenesisBlock;
            Console.WriteLine(JsonConvert.SerializeObject(genesisBlock, Formatting.Indented));
        }

        private void DoLastBlock()
        {
            Console.Clear();
            Console.WriteLine("\n\n\nLast Block");
            Console.WriteLine("Time: {0}", DateTime.Now);
            Console.WriteLine("======================");
            var block = this.bc.GetLastBlock;

            // Console.WriteLine("ID          :{0}", block.ID);
            Console.WriteLine("Height      : {0}", block.Height);
            Console.WriteLine("Timestamp   : {0}", block.TimeStamp.ConvertToDateTime());
            Console.WriteLine("Prev. Hash  : {0}", block.PreviousHash);
            Console.WriteLine("Hash        : {0}", block.Hash);

            var transactions = block.Transactions;
            Console.WriteLine("Transactions:");
            foreach (Transaction trx in transactions)
            {
                Console.WriteLine("   Timestamp   : {0}", trx.TimeStamp.ConvertToDateTime());
                Console.WriteLine("   Sender      : {0}", trx.Sender);
                Console.WriteLine("   Recipient   : {0}", trx.Recipient);
                Console.WriteLine("   Amount      : {0}", trx.Amount.ToString("N", CultureInfo.InvariantCulture));
                Console.WriteLine("   Fee         : {0}", trx.Fee.ToString("N4", CultureInfo.InvariantCulture));
                Console.WriteLine("   - - - - - - ");
            }
        }

        private void DoSendCoin()
        {
            Console.Clear();
            Console.WriteLine("\n\n\n\nSend Money");
            Console.WriteLine("Time: {0}", DateTime.Now);
            Console.WriteLine("======================");

            Console.WriteLine("Please enter the sender name!:");
            Console.WriteLine("(type 'ga1' or 'ga2' for first time)");
            var sender = Console.ReadLine();

            Console.WriteLine("Please enter the recipient name!:");
            var recipient = Console.ReadLine();

            Console.WriteLine("Please enter the amount (number)!:");
            var strAmount = Console.ReadLine();

            Console.WriteLine("Please enter fee (number)!:");
            var strFee = Console.ReadLine();

            // validate input
            if (string.IsNullOrEmpty(sender) ||
                string.IsNullOrEmpty(recipient) ||
                string.IsNullOrEmpty(strAmount) ||
                string.IsNullOrEmpty(strFee))
            {
                Console.WriteLine("\n\nError, Please input all data: sender, recipient, amount and fee!\n");
                return;
            }

            var success = double.TryParse(strAmount, out var amount);

            if (!success)
            {
                Console.WriteLine("\nError! You have inputted the wrong value for  the amount!");
                return;
            }

            success = double.TryParse(strFee, out var fee);

            if (!success)
            {
                Console.WriteLine("\nError! You have inputted the wrong value for the fee!");
                return;
            }

            // validating fee
            // assume max fee is 50% of amount
            if (fee > (0.5 * amount))
            {
                Console.WriteLine("\nError! You have inputted the fee to high, max fee 50% of amount!");
                return;
            }

            // get sender balance
            var senderBalance = this.bc.GetBalance(sender);

            // validate amount and fee
            if ((amount + fee) > senderBalance)
            {
                Console.WriteLine("\nError! Sender ({0}) don't have enough balance!", sender);
                Console.WriteLine("Sender ({0}) balance is {1}", sender, senderBalance);
                return;
            }

            // Create transaction
            var newTrx = new Transaction(sender, recipient, amount, fee);
            this.bc.TransactionsPool.AddTransaction(newTrx);

            Console.Clear();
            Console.WriteLine("\n\n\n\nHoree, transaction added to transaction pool!.");
            Console.WriteLine("Sender: {0}", sender);
            Console.WriteLine("Recipient {0}", recipient);
            Console.WriteLine("Amount: {0}", amount);
            Console.WriteLine("Fee: {0}", fee);
        }

        private void DoCreateBlock()
        {
            Console.Clear();
            Console.WriteLine("\n\n\nCreate Block");
            Console.WriteLine("======================");

            var transactionPool = this.bc.TransactionsPool;

            if (!transactionPool.Transactions.Any())
            {
                Console.WriteLine("No transaction in pool, please create transaction first!");
                return;
            }

            this.bc.AddBlock();
        }

        private void DoGetBalance()
        {
            Console.Clear();
            Console.WriteLine("Get Balance Account");
            Console.WriteLine("Please enter name:");
            string name = Console.ReadLine();

            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("\n\nError, Please input name!\n");
                return;
            }

            Console.Clear();
            Console.WriteLine("Balance for {0}", name);
            Console.WriteLine("Time: {0}", DateTime.Now);
            Console.WriteLine("======================");
            var balance = this.bc.GetBalance(name);
            Console.WriteLine("Balance: {0}", balance.ToString("N", CultureInfo.InvariantCulture));
        }

        private void DoGetTransactionHistory()
        {
            Console.Clear();
            Console.WriteLine("Get Transaction History");
            Console.WriteLine("Please enter name:");
            var name = Console.ReadLine();

            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("\n\nError, Please input name!\n");
                return;
            }

            Console.Clear();
            Console.WriteLine("Transaction History for {0}", name);
            Console.WriteLine("Time: {0}", DateTime.Now);
            Console.WriteLine("======================");

            var transactions = this.bc.Blocks
                .SelectMany(b => b.Transactions)
                .Where(t => t.Recipient == name || t.Sender == name)
                .ToArray();

            if (transactions.Any())
            {
                foreach (var transaction in transactions)
                {
                    Console.WriteLine("Timestamp   : {0}", transaction.TimeStamp.ConvertToDateTime());
                    Console.WriteLine("Sender      : {0}", transaction.Sender);
                    Console.WriteLine("Recipient   : {0}", transaction.Recipient);
                    Console.WriteLine("Amount      : {0}", transaction.Amount.ToString("N", CultureInfo.InvariantCulture));
                    Console.WriteLine("Fee         : {0}", transaction.Fee.ToString("N4", CultureInfo.InvariantCulture));
                    Console.WriteLine("--------------\n");
                }
            }
            else
            {
                Console.WriteLine("\n---- no record found! ---");
            }
        }

        private void DoShowBlockchain()
        {
            Console.Clear();
            Console.WriteLine("\n\n\nBlockchain Explorer");
            Console.WriteLine("Time: {0}", DateTime.Now);
            Console.WriteLine("======================");

            foreach (Block block in this.bc.Blocks)
            {
                // Console.WriteLine("ID          :{0}", block.ID);
                Console.WriteLine("Height      : {0}", block.Height);
                Console.WriteLine("Timestamp   : {0}", block.TimeStamp.ConvertToDateTime());
                Console.WriteLine("Prev. Hash  : {0}", block.PreviousHash.ConvertToHexString());
                Console.WriteLine("Hash        : {0}", block.Hash.ConvertToHexString());

                if (block.Height == 1)
                {
                    Console.WriteLine("Transactions : {0}", block.Transactions);
                }
                else
                {
                    var transactions = block.Transactions;
                    Console.WriteLine("Transactions:");
                    foreach (Transaction trx in transactions)
                    {
                        Console.WriteLine("   Timestamp   : {0}", trx.TimeStamp.ConvertToDateTime());
                        Console.WriteLine("   Sender      : {0}", trx.Sender);
                        Console.WriteLine("   Recipient   : {0}", trx.Recipient);
                        Console.WriteLine("   Amount      : {0}", trx.Amount.ToString("N", CultureInfo.InvariantCulture));
                        Console.WriteLine("   Fee         : {0}", trx.Fee.ToString("N4", CultureInfo.InvariantCulture));
                        Console.WriteLine("   - - - - - - ");
                    }
                }

                Console.WriteLine("--------------\n");
            }
        }

        private void DoExit()
        {
            Console.Clear();
            Console.WriteLine("\n\nApplication closed!\n");
            Environment.Exit(0);
        }
    }
}
