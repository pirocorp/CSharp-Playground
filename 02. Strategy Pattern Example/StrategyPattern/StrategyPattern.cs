namespace StrategyPattern
{
    using System;

    public static class StrategyPattern
    {
        public static void Main()
        {
            //NoStrategyPatternProblem();
            StrategyPatternProblem();
        }

        private static void StrategyPatternProblem()
        {
            var (grossTotal, selection) = ReadInput();

            IDiscountCalculator discountStrategy = selection switch
            {
                1 => new SeniorCitizenDiscount(),
                2 => new VeteranDiscount(),
                3 => new MinorDiscount(),
                _ => new NoDiscount()
            };

            var context = new DiscountContext(discountStrategy);

            var net = context.Calculate(grossTotal);
            PrintResult(grossTotal, net);
        }

        private static void NoStrategyPatternProblem()
        {
            var (grossTotal, selection) = ReadInput();

            var discount = selection switch
            {
                1 => 0.85M,
                2 => 0.9M,
                3 => 0.95M,
                _ => 1.0M
            };

            var net = grossTotal * discount;
            PrintResult(grossTotal, net);
        }
        
        private static (decimal GrossTotal, int Selection) ReadInput()
        {
            var rnd = new Random();

            // Get 'random' number between 100 and 1_000
            var grossTotal = rnd.Next(100, 1_000);

            var selection = 0;

            while (selection > 4 || selection <= 0)
            {
                Console.Clear();
                Console.WriteLine($"The value of the bill is: {grossTotal}. Which discount would you like to use?");
                Console.WriteLine($"1. Senior citizen");
                Console.WriteLine($"2. Veteran");
                Console.WriteLine($"3. Minor");
                Console.WriteLine($"4. No discount");

                int.TryParse(Console.ReadKey().KeyChar.ToString(), out selection);
            }

            return (grossTotal, selection);
        }

        private static void PrintResult(decimal grossTotal, decimal net)
        {
            Console.WriteLine();
            Console.WriteLine($"The gross total was: {grossTotal:F2}");
            Console.WriteLine($"The discount is: {(grossTotal - net):F2}");
            Console.WriteLine($"The net total is: {net:F2}");
        }
    }
}
