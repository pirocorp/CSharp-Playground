namespace StrategyPattern
{
    public class SeniorCitizenDiscount : IDiscountCalculator
    {
        private const decimal discount = 0.85M;

        public decimal CalculateDiscount(decimal amount) => amount * discount;
    }
}
