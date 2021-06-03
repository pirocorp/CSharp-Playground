namespace Coin.PoS
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Stake
    {
        private List<Staker> stackers;

        public Stake()
        {
            this.stackers = new List<Staker>();
        }

        public List<Staker> StakerList => this.stackers = new List<Staker>(this.stackers);

        public void Add(string address, double amount)
            => this.stackers.Add(new Staker(address, amount));

        // Select random market maker as block maker
        public string GetBlockValidator()
        {
            var random = new Random();
            var stacker = this.stackers[random.Next(0, this.stackers.Count)];

            return stacker.Address;
        }

        private void Initialize()
        {
            // All ICO are assumed to stake their coin
            var marketMakers = Ico.Accounts.Select(s => new Staker(s.Address, s.Balance));

            this.stackers.AddRange(marketMakers);
        }
    }
}
