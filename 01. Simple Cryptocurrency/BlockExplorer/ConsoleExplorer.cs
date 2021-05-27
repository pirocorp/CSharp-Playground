namespace BlockExplorer
{
    using System;
    using System.Globalization;
    using System.Threading;
    using GrpcService;
    using Utilities;
    using static GrpcService.BChainService;

    public class ConsoleExplorer
    {
        private readonly BChainServiceClient service;

        public ConsoleExplorer(BChainServiceClient service)
        {
            this.service = service;
        }

        public void Run()
        {
            this.MenuItem();
            this.GetInput();
        }

        private void MenuItem()
        {
            Console.Clear();
            Console.WriteLine("\n\n\n");
            Console.WriteLine("                    COIN EXPLORER ");
            Console.WriteLine("============================================================");
            Console.WriteLine("                    1. First Block");
            Console.WriteLine("                    2. Last Block");
            Console.WriteLine("                    3. Show All Block");
            Console.WriteLine("                    9. Exit");
            Console.WriteLine("------------------------------------------------------------");
        }

        private void GetInput()
        {
            var selection = 0;

            while (selection != 20)
            {
                switch (selection)
                {
                    case 1:
                        this.ShowFirstBlock();
                        break;
                    case 2:
                        this.ShowLastBlock();
                        break;
                    case 3:
                        this.ShowAllBlocks();
                        break;
                    case 9:
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
                        this.MenuItem();
                    }
                }

                Console.WriteLine("\n**** Please select menu!!! *****");
                var input = Console.ReadLine();
                var success = int.TryParse(input, out selection);

                if (!success)
                {
                    selection = 0;
                    Console.Clear();
                    this.MenuItem();
                }
            }
        }

        private static void PrintBlock(BlockModel block)
        {
            Console.WriteLine("Height      : {0}", block.Height);
            Console.WriteLine("Timestamp   : {0}", block.TimeStamp.ConvertToDateTime());
            Console.WriteLine("Hash        : {0}", block.Hash);
            Console.WriteLine("Prev. Hash  : {0}", block.PrevHash);

            var transactions = block.Transactions;
            Console.WriteLine("Transactions:");
            foreach (var trx in transactions)
            {
                Console.WriteLine("   ID          : {0}", trx.TransactionID);
                Console.WriteLine("   Timestamp   : {0}", trx.TimeStamp.ConvertToDateTime());
                Console.WriteLine("   Sender      : {0}", trx.Sender);
                Console.WriteLine("   Recipient   : {0}", trx.Recipient);
                Console.WriteLine("   Amount      : {0}", trx.Amount.ToString("N", CultureInfo.InvariantCulture));
                Console.WriteLine("   Fee         : {0}", trx.Fee.ToString("N4", CultureInfo.InvariantCulture));
                Console.WriteLine("   - - - - - - ");

            }
        }

        private void ShowFirstBlock()
        {
            Console.Clear();
            Console.WriteLine("\n\n\n\nGenesis Block");
            Console.WriteLine("Time: {0}", DateTime.Now);
            Console.WriteLine("======================");
            var response = this.service.GenesisBlock(new EmptyRequest());
            var block = response.Block;

            PrintBlock(block);

            Console.WriteLine("--------------\n");
        }

        private void ShowLastBlock()
        {
            Console.Clear();
            Console.WriteLine("\n\n\n\nLast Block");
            Console.WriteLine("Time: {0}", DateTime.Now);
            Console.WriteLine("======================");
            var response = this.service.LastBlock(new EmptyRequest());
            var block = response.Block;

            PrintBlock(block);

            Console.WriteLine("--------------\n");
        }

        private void ShowAllBlocks()
        {
            Console.Clear();
            Console.WriteLine("\n\n\nBlockchain Explorer");
            Console.WriteLine("Time: {0}", DateTime.Now);
            Console.WriteLine("======================");

            Console.WriteLine("\nPlease enter the page number!:");
            string strPageNumber = Console.ReadLine();

            // validate input
            if (string.IsNullOrEmpty(strPageNumber))
            {

                Console.WriteLine("\n\nError, Please input page number!\n");
                return;
            }

            var success = int.TryParse(strPageNumber, out var pageNumber);

            if (!success)
            {
                Console.WriteLine("\n\nError, Please input number!\n");
                return;
            }

            try
            {
                var response = this.service.GetBlocks(new BlockRequest
                {
                    PageNumber = pageNumber,
                    ResultPerPage = 5
                });

                foreach (var block in response.Blocks)
                {
                    PrintBlock(block);

                    Console.WriteLine("--------------\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void DoExit()
        {
            Console.Clear();
            Console.WriteLine("\n\nApplication closed!\n");
            Thread.Sleep(2000);
            Environment.Exit(0);
        }
    }
}
