namespace Coin.Tests
{
    using System;
    using System.Collections.Generic;
    using Utils;
    using Xunit;

    public class BlockUnitTest
    {
        [Fact]
        public void NewBlockTest()
        {
            var lsTrx = new List<Transaction>();

            //Create first transaction
            var trx1 = new Transaction("Johana", "Merlin", 3.0, 0.3);

            lsTrx.Add(trx1);

            var block = new Block(0, string.Empty, lsTrx.ToArray(), "Admin");
            Assert.Equal(0, block.Height);
            Assert.Equal(string.Empty, block.PreviousHash);
            Assert.Equal("Admin", block.Validator);
            Assert.Equal(lsTrx, block.Transactions);
            Assert.Single(block.Transactions);
            Assert.Equal(32, block.Hash.Length);
        }
    }
}
