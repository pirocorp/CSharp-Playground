namespace Coin.PoS
{
    public class Staker
    {
        public Staker(string address, double amount)
        {
            this.Address = address;
            this.Amount = amount;
        }

        public string Address { get; }

        public double Amount { get; }
    }
}
