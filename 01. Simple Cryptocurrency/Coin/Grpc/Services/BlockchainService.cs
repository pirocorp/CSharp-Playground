namespace Coin.Grpc.Services
{
    using System.Linq;
    using System.Threading.Tasks;

    using Coin.Utils;
    using global::Grpc.Core;
    using GrpcService;

    public class BlockchainService : BChainService.BChainServiceBase
    {
        private readonly IBlockChain blockChain;

        public BlockchainService(IBlockChain blockChain)
        {
            this.blockChain = blockChain;
        }

        public override Task<BlockResponse> GenesisBlock(EmptyRequest request, ServerCallContext context)
        {
            var block = this.blockChain.GetGenesisBlock;

            var model = ConvertBlockToBlockModel(block);

            return Task.FromResult(new BlockResponse
            {
                Block = model,
            });
        }

        public override Task<BalanceResponse> GetBalance(AccountRequest request, ServerCallContext context)
        {
            var balance = this.blockChain.GetBalance(request.Address);

            return Task.FromResult(new BalanceResponse
            {
                Balance = balance,
            });
        }

        public override Task<BlockResponse> LastBlock(EmptyRequest request, ServerCallContext context)
        {
            var block = this.blockChain.GetLastBlock;
            var model = ConvertBlockToBlockModel(block);

            return Task.FromResult(new BlockResponse
            {
                Block = model,
            });
        }

        public override Task<BlocksResponse> GetBlocks(BlockRequest request, ServerCallContext context)
        {
            var blocks = this.blockChain.GetBlocks(request.PageNumber, request.ResultPerPage);

            var response = new BlocksResponse();

            foreach (var block in blocks)
            {
                response.Blocks.Add(ConvertBlockToBlockModel(block));
            }

            return Task.FromResult(response);
        }

        public override Task<TransactionsResponse> GetTransactions(AccountRequest request, ServerCallContext context)
        {
            var response = new TransactionsResponse();

            var transactions = this.blockChain
                .GetTransactions(request.Address)
                .Select(t => new TransactionModel()
                {
                    TransactionID = t.Id,
                    TimeStamp = t.TimeStamp,
                    Sender = t.Sender,
                    Recipient = t.Recipient,
                    Amount = t.Amount,
                    Fee = t.Fee,
                });

            foreach (var trx in transactions)
            {
                response.Transactions.Add(trx);
            }

            return Task.FromResult(response);
        }

        public override Task<TransactionResponse> SendCoin(SendRequest request, ServerCallContext context)
        {
            // Create new transaction
            var newTrx = new Transaction(
                request.TransactionInput.SenderAddress,
                request.TransactionOutput.RecipientAddress,
                request.TransactionOutput.Amount,
                request.TransactionOutput.Fee) { Id = request.TransactionId };

            // verify transaction ID
            var trxHash = TransactionHelpers.GetTransactionHash(newTrx);

            if (!trxHash.Equals(request.TransactionId))
            {
                return Task.FromResult(new TransactionResponse
                {
                    Result = "Transaction ID not valid",
                });
            }

            // Verify signature
            var isValid = TransactionHelpers.VerifySignature(request.PublicKey, request.TransactionId, request.TransactionInput.Signature);
            if (!isValid)
            {
                return Task.FromResult(new TransactionResponse()
                {
                    Result = "Signature not valid",
                });
            }

            this.blockChain.AddTransaction(newTrx);

            return Task.FromResult(new TransactionResponse()
            {
                Result = "Success",
            });
        }

        private static BlockModel ConvertBlockToBlockModel(Block block)
        {
            var model = new BlockModel
            {
                Height = block.Height,
                Hash = block.Hash.ConvertToHexString(),
                PrevHash = block.PreviousHash.ConvertToHexString(),
                TimeStamp = block.TimeStamp,
            };

            var transactions = block.Transactions.Select(t => new TransactionModel()
            {
                TransactionID = t.Id,
                TimeStamp = t.TimeStamp,
                Sender = t.Sender,
                Recipient = t.Recipient,
                Amount = t.Amount,
                Fee = t.Fee,
            });

            foreach (var transaction in transactions)
            {
                model.Transactions.Add(transaction);
            }

            return model;
        }
    }
}
