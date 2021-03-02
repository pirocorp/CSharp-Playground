namespace StrategyPattern
{
    public class DiscountContext
    {
        private readonly IDiscountCalculator strategy;

        public DiscountContext(IDiscountCalculator strategy)
        {
            this.strategy = strategy;
        }

        public decimal Calculate(decimal amount) => this.strategy.CalculateDiscount(amount);
    }
}
