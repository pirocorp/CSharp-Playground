namespace CSharpCurrying
{
    using System;

    /// <summary>
    /// Currying effectively decomposes the function into functions taking a single parameter.
    /// </summary>
    public static class Program
    {
        public static void Main()
        {
            var curriedFunction = CurryingExample.CurriedAdd;

            var twelve = curriedFunction(3)(4)(5);
            Console.WriteLine(twelve); //12

            var oneParam = curriedFunction(3);
            var twoParams = oneParam(4);
            twelve = twoParams(5);

            Console.WriteLine(twelve); // 12

            var curriedAction = CurryingExample.CurriedAction;

            var oneAction = curriedAction(3);
            var twoAction = oneAction(4);
            twoAction(5);  // prints 12
        }
    }
}
