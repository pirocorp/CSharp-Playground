namespace CSharpFunctionComposition
{
    using System;

    public static class Program
    {
        public static void Main()
        {
            Func<double, double> sqr = (x) => Math.Pow(x, 2);
            Func<double, double> triple = (z) => 3 * z;

            //3*x^2
            var func = FunctionCompositionExample.Compose(sqr, triple);

            Console.WriteLine(func(5)); // 3*5^2 = 75
        }
    }
}
