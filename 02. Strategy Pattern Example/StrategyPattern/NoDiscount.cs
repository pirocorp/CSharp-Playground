namespace StrategyPattern
{
    public class NoDiscount : IDiscountCalculator
    {
        public decimal CalculateDiscount(decimal amount) => amount;
    }
}
