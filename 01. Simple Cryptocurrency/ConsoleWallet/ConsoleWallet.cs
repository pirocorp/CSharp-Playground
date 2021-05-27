namespace ConsoleWallet
{
    using System;
    using System.Globalization;
    using System.Threading;

    using global::ConsoleWallet.Utilities;
    using GrpcService;

    using static GrpcService.BChainService;

    public class ConsoleWallet
    {
        private readonly BChainServiceClient service;
        private Account account;

        public ConsoleWallet(BChainServiceClient service)
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
            if (this.account is null)
            {
                Console.Clear();
                Console.WriteLine("\n\n\n");
                Console.WriteLine("                    COIN WALLET ");
                Console.WriteLine("============================================================");
                Console.WriteLine("  Address: - ");
                Console.WriteLine("============================================================");
                Console.WriteLine("                    1. Create Account");
                Console.WriteLine("                    2. Restore Account");
                Console.WriteLine("                    9. Exit");
                Console.WriteLine("------------------------------------------------------------");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("\n\n\n");
                Console.WriteLine("                    COIN WALLET ");
                Console.WriteLine("============================================================");
                Console.WriteLine("  Address: {0}", this.account.Address);
                Console.WriteLine("============================================================");
                Console.WriteLine("                    1. Create Account");
                Console.WriteLine("                    2. Restore Account");
                Console.WriteLine("                    3. Send Coin");
                Console.WriteLine("                    4. Check Balance");
                Console.WriteLine("                    5. Transaction History");
                Console.WriteLine("                    9. Exit");
                Console.WriteLine("------------------------------------------------------------");
            }
        }

        private void GetInput()
        {
            var selection = 0;

            while (selection != 20)
            {
                switch (selection)
                {
                    case 1:
                        this.DoCreateAccount();
                        break;
                    case 2:
                        this.DoRestore();
                        break;
                    case 3:
                        this.DoSendCoin();
                        break;
                    case 4:
                        this.DoGetBalance();
                        break;
                    case 5:
                        this.DoGetTransactionHistory();
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
                var success = int.TryParse(Console.ReadLine(), out selection);

                if (!success)
                {
                    selection = 0;
                    Console.Clear();
                    this.MenuItem();
                }
            }
        }

        private void WalletInfo()
        {
            Console.Clear();
            Console.WriteLine("\n\n\nYour Wallet");
            Console.WriteLine("======================");
            Console.WriteLine("\nADDRESS:\n{0}", this.account.Address);
            Console.WriteLine("\nPUBLIC KEY:\n{0}", this.account.PublicKey);
            Console.WriteLine("\nSECREET NUMBER:\n{0}", this.account.SecretNumber);
            Console.WriteLine("\n - - - - - - - - - - - - - - - - - - - - - - ");
            Console.WriteLine("*** Save secret number!                   ***");
            Console.WriteLine("*** Use secret number to restore account! ***");
        }

        private void DoCreateAccount()
        {
            this.account = new Account();
            this.WalletInfo();
        }

        private void DoRestore()
        {
            Console.Clear();
            Console.WriteLine("Restore Wallet");
            Console.WriteLine("Please enter Secret number:");

            var secret = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(secret))
            {
                Console.WriteLine("\n\nError, Please input secret number!\n");
                return;
            }

            this.account = new Account(secret);
            this.WalletInfo();
        }

        private void DoSendCoin()
        {
            Console.Clear();
            Console.WriteLine("\n\n\n\nTransfer Coin");
            Console.WriteLine("Time: {0}", DateTime.Now);
            Console.WriteLine("======================");

            Console.WriteLine("Sender address:");
            string sender = this.account.Address;
            Console.WriteLine(sender);

            Console.WriteLine("\nPlease enter the recipient address!:");
            string recipient = Console.ReadLine();

            Console.WriteLine("\nPlease enter the amount (number)!:");
            string strAmount = Console.ReadLine();

            Console.WriteLine("\nPlease enter fee (number)!:");
            string strFee = Console.ReadLine();
            double amount;

            if (string.IsNullOrEmpty(sender) ||
                string.IsNullOrEmpty(recipient) ||
                string.IsNullOrEmpty(strAmount) ||
                string.IsNullOrEmpty(strFee))
            {
                Console.WriteLine("\n\nError, Please input all data: sender, recipient, amount and fee!\n");
                return;
            }

            var success = double.TryParse(strAmount, out amount);

            if (!success)
            {
                Console.WriteLine("\nError! You have inputted the wrong value for  the amount!");
                return;
            }

            success = float.TryParse(strFee, out var fee);

            if (!success)
            {
                Console.WriteLine("\nError! You have inputted the wrong value for the fee!");
                return;
            }

            var response = this.service.GetBalance(new AccountRequest
            {
                Address = sender,
            });

            var senderBalance = response.Balance;

            if ((amount + fee) > senderBalance)
            {
                Console.WriteLine("\nError! Sender ({0}) don't have enough balance!", sender);
                Console.WriteLine("Sender ({0}) balance is {1}", sender, senderBalance);
                return;
            }

            var transactionInput = new TransactionInput()
            {
                SenderAddress = this.account.Address,
                TimeStamp = DateTime.Now.Ticks,
            };

            var transactionOutput = new TransactionOutput()
            {
                RecipientAddress = recipient,
                Amount = amount,
                Fee = fee,
            };

            var transactionHash = TransactionHelpers.GetTransactionHash(transactionInput, transactionOutput);
            var signature = this.account.CreateSignature(transactionHash);

            transactionInput.Signature = signature;

            var sendRequest = new SendRequest
            {
                TransactionId = transactionHash,
                PublicKey = this.account.PublicKey,
                TransactionInput = transactionInput,
                TransactionOutput = transactionOutput,
            };

            try
            {
                var responseSend = this.service.SendCoin(sendRequest);

                if (responseSend.Result.ToLower() == "success")
                {
                    Console.Clear();
                    Console.WriteLine("\n\n\n\nTransaction has send to Blockchain.!.");
                    Console.WriteLine("Sender: {0}", sender);
                    Console.WriteLine("Recipient {0}", recipient);
                    Console.WriteLine("Amount: {0}", amount);
                    Console.WriteLine("Fee: {0}", fee);
                    Console.WriteLine("-------------------");
                    Console.WriteLine("Need around 30 second to be processed!");
                }
                else
                {
                    Console.WriteLine("Error: {0}", responseSend.Result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }
        }

        private void DoGetBalance()
        {
            var address = this.account.Address;

            if (string.IsNullOrEmpty(address))
            {
                Console.WriteLine("\n\nError, Address empty, please create account first!\n");
                return;
            }

            Console.Clear();
            Console.WriteLine("Balance for {0}", address);
            Console.WriteLine("Time: {0}", DateTime.Now);
            Console.WriteLine("======================");
            try
            {
                var response = this.service.GetBalance(new AccountRequest
                {
                    Address = address,
                });

                Console.WriteLine("Balance: {0}", response.Balance.ToString("N", CultureInfo.InvariantCulture));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void DoGetTransactionHistory()
        {
            var address = this.account.Address;

            if (string.IsNullOrEmpty(address))
            {
                Console.WriteLine("\n\nError, Address empty, please create account first!\n");
                return;
            }

            Console.Clear();
            Console.WriteLine("Transaction History for {0}", address);
            Console.WriteLine("Time: {0}", DateTime.Now);
            Console.WriteLine("======================");

            try
            {
                var response = this.service.GetTransactions(new AccountRequest
                {
                    Address = address,
                });

                if (response?.Transactions != null)
                {
                    foreach (var trx in response.Transactions)
                    {
                        Console.WriteLine("ID          : {0}", trx.TransactionID);
                        Console.WriteLine("Timestamp   : {0}", trx.TimeStamp.ConvertToDateTime());
                        Console.WriteLine("Sender      : {0}", trx.Sender);
                        Console.WriteLine("Recipient   : {0}", trx.Recipient);
                        Console.WriteLine("Amount      : {0}", trx.Amount.ToString("N", CultureInfo.InvariantCulture));
                        Console.WriteLine("Fee         : {0}", trx.Fee.ToString("N4", CultureInfo.InvariantCulture));
                        Console.WriteLine("--------------\n");

                    }
                }
                else
                {
                    Console.WriteLine("\n---- no record found! ---");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
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
