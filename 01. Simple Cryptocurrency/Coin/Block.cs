#pragma warning disable SA1124 // Do not use regions
namespace Coin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    using Coin.Utils;

    public class Block
    {
        private readonly Transaction[] transactions;

        public Block(long height, string previousHash, IEnumerable<Transaction> transactions, string validator)
        {
            this.Version = 1;
            this.PreviousHash = previousHash;
            this.TimeStamp = DateTime.UtcNow.Ticks;
            this.MerkleRoot = this.GenerateMerkleRoot();
            this.Height = height;
            this.transactions = transactions.ToArray();
            this.Hash = this.GenerateBlockHash();
            this.Difficulty = 1;
            this.Validator = validator;
        }

        #region BlockHeader

        public int Version { get; set; }

        // The hash of the block. The hash act as the unique identity of the given block in the blockchain.
        public string PreviousHash { get; private set; }

        // The root hash of Merkle Tree (Hash Tree). The MerkleRoot will be calculated for Transactions.
        public string MerkleRoot { get; set; }

        // The timeStamp is the time when the block was created.
        public long TimeStamp { get; private set; }

        public int Difficulty { get; set; }

        #endregion

        // The sequence amount of blocks.
        public long Height { get; private set; }

        // Hash is the hash of the previous block.
        public string Hash { get; private set; }

        // Transactions are collections of transactions that occur, as previously discussed above.
        public IEnumerable<Transaction> Transactions => this.transactions.ToList().AsReadOnly();

        // The creator of the block identified by the public key.
        // Validators get reward from accumulated transaction fees.
        public string Validator { get; private set; }

        public int TransactionsCount => this.transactions.Length;

        public double TotalAmount => this.transactions.Sum(t => t.Amount);

        public double TotalReward => this.transactions.Sum(t => t.Fee);

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"Height       :{this.Height}");
            sb.AppendLine($"Timestamp    :{this.TimeStamp.ConvertToDateTime()}");
            sb.AppendLine($"Prev. Hash   :{this.PreviousHash}");
            sb.AppendLine($"Hash         :{this.Hash}");
            sb.AppendLine($"Transactions :{this.Transactions.ConvertToString()}");
            sb.AppendLine($"Creator      :{this.Validator}");
            sb.AppendLine("-----------------");

            return sb.ToString();
        }

        private static byte[] HexToBytes(string hex)
            => Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();

        private static string BytesToHex(byte[] bytes)
            => Convert.ToHexString(bytes).ToLower();

        private static string DoubleHash(string left, string right)
        {
            var leftByte = HexToBytes(left);
            var rightByte = HexToBytes(right);

            var concatHash = leftByte.Concat(rightByte).ToArray();
            var sha256 = SHA256.Create();
            var sendHash = sha256.ComputeHash(sha256.ComputeHash(concatHash));

            return BytesToHex(sendHash).ToLower();
        }

        private static string CreateMerkleRoot(IList<string> txsHash)
        {
            while (true)
            {
                if (txsHash.Count == 0)
                {
                    return string.Empty;
                }

                if (txsHash.Count == 1)
                {
                    return txsHash[0];
                }

                var newHashList = new List<string>();

                var length = (txsHash.Count % 2 != 0) ? txsHash.Count - 1 : txsHash.Count;

                for (var i = 0; i < length; i += 2)
                {
                    newHashList.Add(DoubleHash(txsHash[i], txsHash[i + 1]));
                }

                if (length < txsHash.Count)
                {
                    newHashList.Add(DoubleHash(txsHash[^1], txsHash[^1]));
                }

                txsHash = newHashList.ToArray();
            }
        }

        private string GenerateBlockHash()
        {
            var blockData =
                this.Version
                + this.PreviousHash
                + this.MerkleRoot
                + this.TimeStamp
                + this.Difficulty
                + this.Validator;

            return blockData.GenerateHash();
        }

        private string GenerateMerkleRoot()
        {
            var transactionHashes = this.transactions.Select(transaction => transaction.Hash).ToList();

            var merkleHash = CreateMerkleRoot(transactionHashes);
            return merkleHash;
        }
    }
}
