namespace Coin.Tests
{
    using Utils;
    using Xunit;

    public class BlockchainUnitTest
    {
        [Fact]
        public void CreateGenesisBlockTest()
        {
            var bc = new BlockChain();
            var genesisBlock = bc.GetGenesisBlock;
            Assert.Equal(1, bc.Count);
        }

        [Fact]
        public void GetLastBlockTest()
        {
            var bc = new BlockChain();
            //Last block is genesis for first time
            var lastBlock = bc.GetLastBlock;
            Assert.Equal(1, lastBlock.Height);
            Assert.Equal(string.Empty, lastBlock.PreviousHash);
            Assert.Equal(32, lastBlock.Hash.Length);
        }

        [Fact]
        public void AddBlockTest()
        {
            var bc = new BlockChain();

            //Create first transaction
            var transaction1 = new Transaction("Stevano", "Valentino", 3.0, 0.3);
            //{
            //    Sender = "Stevano",
            //    Recipient = "Valentino",
            //    Amount = 3.0,
            //    Fee = 0.3
            //};

            bc.TransactionsPool.AddTransaction(transaction1);
            bc.AddBlock();
            Assert.Equal(2, bc.Count);

            //Create 2nd transaction
            var transaction2 = new Transaction("Budiawan", "Norita", 2.5, 0.2);
            //{
            //    Sender = "Budiawan",
            //    Recipient = "Norita",
            //    Amount = 2.5,
            //    Fee = 0.2
            //};

            bc.TransactionsPool.AddTransaction(transaction2);
            bc.AddBlock();
            Assert.Equal(3, bc.Count);
        }
    }
}
