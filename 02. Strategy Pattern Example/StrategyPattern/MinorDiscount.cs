namespace StrategyPattern
{
    public class MinorDiscount : IDiscountCalculator
    {
        private const decimal discount = 0.95M;

        public decimal CalculateDiscount(decimal amount) => amount * discount;
    }
}