namespace Techniques
{
    using System;

    using static Functional.Techniques;

    public static class Program
    {
        // Currying example with lambda
        private static readonly Func<int, Func<int, int>> Sum = x => y => x + y;
       
        // Currying example with full method
        public static Func<int, int> SumFull(int x)
        {
            int partial(int y)
            {
                return x + y;
            }

            return partial;
        }

        public static void Main()
        {
            // Currying
            Console.WriteLine(Sum(2)(3));
            Console.WriteLine(SumFull(5)(9));

            // Func<int, int, int, int> func = (x, y, z) => x + y + z;
            var function = Func((int x, int y, int z) => x + y + z);

            var curriedFunction = Curry(function);

            var curriedOne = curriedFunction(1);
            var curriedTwo = curriedOne(2);
            var curryResult = curriedTwo(3);

            Console.WriteLine(curryResult);

            // Partial application
            var partialFunction = Partial(function, 2, 3);

            var partialResult = partialFunction(4);

            Console.WriteLine(partialResult);

            // Composition
            var firstFunction = Func((double number) => (int)Math.Round(number));
            var secondFunction = Func((int integer) => integer % 2 == 0);

            var composedFunction = Compose(firstFunction, secondFunction);

            var composedResult = composedFunction(1.2);

            Console.WriteLine(composedResult);
        }
    }
}
