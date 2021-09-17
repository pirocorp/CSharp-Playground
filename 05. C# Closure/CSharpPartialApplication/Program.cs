namespace CSharpPartialApplication
{
    using System;

    /// <summary>
    /// Very simply, partial application lets you assign the first n parameters,
    /// returning a function that takes the rest. Given a function that returns
    /// the sum of three values:
    /// </summary>
    public static class Program
    {
        public static void Main()
        {
            var a = 3;
            var b = 5;
            var c = 8;

            var noPartialFunc = PartialApplicationExample.Add;
            var partialFuncA = PartialApplicationExample.AddPartial(a);
            var partialFuncAB = PartialApplicationExample.AddPartial(a, b);

            Console.WriteLine(noPartialFunc(a, b, c));
            Console.WriteLine(partialFuncA(b, c));
            Console.WriteLine(partialFuncAB(c));
        }
    }
}
