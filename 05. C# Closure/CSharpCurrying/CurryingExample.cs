namespace CSharpCurrying
{
    using System;

    public static class CurryingExample
    {
        /// <summary>
        /// Returns Curried function that sums three parameters.
        /// </summary>
        public static Func<int, Func<int, Func<int, int>>> CurriedAdd = a => b => c => a + b + c;

        /// <summary>
        /// Returns Curried action that sums three parameters and prints the sum on the console.
        /// </summary>
        public static Func<int, Func<int, Action<int>>> CurriedAction = a => b => c => Console.WriteLine(a + b + c);
    }
}
