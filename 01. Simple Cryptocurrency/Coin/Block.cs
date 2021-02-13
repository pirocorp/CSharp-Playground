namespace Coin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    public class Block
    {
        private readonly Transaction[] transactions;

        public Block(int height, byte[] previousHash, IEnumerable<Transaction> transactions, string creator)
        {
            this.Height = height;
            this.PreviousHash = previousHash;
            this.TimeStamp = DateTime.Now.Ticks;
            this.transactions = transactions.ToArray();
            this.Hash = this.GenerateHash();
            this.Creator = creator;
        }

        // The sequence amount of blocks.
        public int Height { get; private set; }

        // The timeStamp is the time when the block was created.
        public long TimeStamp { get; private set; }

        // The hash of the block. The hash act as the unique identity of the given block in the blockchain.
        public byte[] PreviousHash { get; private set; }

        // Hash is the hash of the previous block.
        public byte[] Hash { get; private set; }

        // Transactions are collections of transactions that occur, as previously discussed above.
        public IEnumerable<Transaction> Transactions => this.transactions.ToList().AsReadOnly();

        // The creator is the creator of the block identified by the public key.
        public string Creator { get; private set; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"Height       :{this.Height}");
            sb.AppendLine($"Timestamp    :{this.TimeStamp.ConvertToDateTime()}");
            sb.AppendLine($"Prev. Hash   :{this.PreviousHash.ConvertToHexString()}");
            sb.AppendLine($"Hash         :{this.Hash.ConvertToHexString()}");
            sb.AppendLine($"Transactions :{this.Transactions.ConvertToString()}");
            sb.AppendLine($"Creator      :{this.Creator}");
            sb.AppendLine("-----------------");

            return sb.ToString();
        }

        private byte[] GenerateHash()
        {
            var sha = SHA256.Create();

            // Convert timestamp to byte array.
            var timeStamp = BitConverter.GetBytes(this.TimeStamp);

            // Convert transactions to byte array.
            var transactionHash = this.Transactions.ToArray().ConvertToByte();

            // Create a new headerBytes array with a length equal to the sum of the lengths of timestamp, transactions, and PreviousHash arrays.
            // In the blockchain, PreviousHash, TimeStamp, and Hash are block headers, while Transactions are the body.
            var headerBytes = new byte[timeStamp.Length + this.PreviousHash.Length + transactionHash.Length];

            // Copy arrays of timestamp, transactions, and previous hash to headerBytes.
            Buffer.BlockCopy(timeStamp, 0, headerBytes, 0, timeStamp.Length);
            Buffer.BlockCopy(this.PreviousHash, 0, headerBytes, timeStamp.Length, this.PreviousHash.Length);
            Buffer.BlockCopy(transactionHash, 0, headerBytes, timeStamp.Length + this.PreviousHash.Length, transactionHash.Length);

            // The function returns the calculated hash of headerBytes.
            return sha.ComputeHash(headerBytes);
        }
    }
}
