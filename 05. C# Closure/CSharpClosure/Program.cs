namespace CSharpClosure
{
    using System;

    // Closure - function retains variables used from its enclosing scope.
    // Partial application - produces function of arbitrary number of arguments
    // Currying - process of converting none unary function into unary function returning unary function.
    // Curry function - unary function returning unary function.
    // Function Composition - creates one function from many functions.

    public static class Program
    {
        public static void Main()
        {
            var i = ClosureExamples.Incrementator();

            while (i() < 5)
            {
                Console.WriteLine("Hi");
            }
        }
    }
}
