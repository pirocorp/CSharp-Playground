namespace Coin.Tests
{
    using System.Collections.Generic;
    using Utils;
    using Xunit;

    public class TransactionUnitTest
    {
        [Fact]
        public void CreateTransactionTest()
        {
            var trx = new Transaction("Jhon", "Wati", 90, 0.001);
            //{
            //    Sender = "Jhon",
            //    Recipient = "Wati",
            //    Amount = 90,
            //    Fee = 0.0001
            //};

            Assert.Equal("Wati", trx.Recipient);
            Assert.Equal("Jhon", trx.Sender);
            Assert.Equal(90, trx.Amount);
        }

        [Fact]
        public void ListTransactionTest()
        {
            //create list of transactions
            var lsTrx = new List<Transaction>();

            //Create first transaction
            var trx1 = new Transaction("Johana", "Merlin", 3.0, 0.3);
            //{
            //    Sender = "Johana",
            //    Recipient = "Merlin",
            //    Amount = 3.0,
            //    Fee = 0.3
            //};

            //Create 2nd transaction
            var trx2 = new Transaction("Budiawan", "Norita", 2.5, 0.2);
            //{
            //    Sender = "Budiawan",
            //    Recipient = "Norita",
            //    Amount = 2.5,
            //    Fee = 0.2
            //};

            //Create 3nd transaction
            var trx3 = new Transaction("Palentino", "Stepano", 1.5, 0.02);
            //{
            //    Sender = "Palentino",
            //    Recipient = "Stepano",
            //    Amount = 1.5,
            //    Fee = 0.02
            //};

            lsTrx.Add(trx1);
            lsTrx.Add(trx2);
            lsTrx.Add(trx3);

            //Test size of list
            Assert.Equal(3, lsTrx.Count);
        }

        [Fact]
        public void ListToBytesTest()
        {
            //create list of transactions
            var lsTrx = new List<Transaction>();

            //Create first transaction
            var trx1 = new Transaction("Johana", "Merlin", 3.0, 0.3);
            //{
            //    Sender = "Johana",
            //    Recipient = "Merlin",
            //    Amount = 3.0,
            //    Fee = 0.3
            //};

            //Create 2nd transaction
            var trx2 = new Transaction("Budiawan", "Norita", 2.5, 0.2);
            //{
            //    Sender = "Budiawan",
            //    Recipient = "Norita",
            //    Amount = 2.5,
            //    Fee = 0.2
            //};

            lsTrx.Add(trx1);
            lsTrx.Add(trx2);


            var transactionBytes = lsTrx.ToArray().ConvertToByte();
            Assert.NotEmpty(transactionBytes);
        }
    }
}
