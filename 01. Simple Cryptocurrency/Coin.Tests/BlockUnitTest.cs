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

            var block = new Block(0, String.Empty.ConvertToBytes(), lsTrx.ToArray(), "Admin");
            Assert.Equal(0, block.Height);
            Assert.Equal(string.Empty.ConvertToBytes(), block.PreviousHash);
            Assert.Equal("Admin", block.Creator);
            Assert.Equal(lsTrx, block.Transactions);
            Assert.Single(block.Transactions);
            Assert.Equal(32, block.Hash.Length);
        }
    }
}
