namespace StrategyPattern
{
    public class VeteranDiscount : IDiscountCalculator
    {
        private const decimal discount = 0.9M;

        public decimal CalculateDiscount(decimal amount) => amount * discount;
    }
}