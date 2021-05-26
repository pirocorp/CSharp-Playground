namespace Coin.Scheduler
{
    using System.Threading.Tasks;

    using Coravel.Invocable;

    public class BlockJob : IInvocable
    {
        private readonly IBlockChain blockChain;

        public BlockJob(IBlockChain blockChain)
        {
            this.blockChain = blockChain;
        }

        public Task Invoke()
        {
            this.blockChain.AddBlock();

            return Task.CompletedTask;
        }
    }
}
