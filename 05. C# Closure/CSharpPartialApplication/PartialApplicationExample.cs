namespace CSharpPartialApplication
{
    using System;

    public static class PartialApplicationExample
    {
        /// <summary>
        /// Returns function that sums three parameters.
        /// </summary>
        public static Func<int, int, int, int> Add => (a, b, c) => a + b + c;

        /// <summary>
        /// Partially apply one parameter.
        /// </summary>
        /// <param name="a">Parameter A</param>
        /// <returns>Sum of all three parameters.</returns>
        public static Func<int, int, int> AddPartial(int a) => (b, c) => a + b + c;

        /// <summary>
        /// Partially apply two parameters.
        /// </summary>
        /// <param name="a">Parameter A</param>
        /// <param name="b">Parameter B</param>
        /// <returns>Sum of all three parameters.</returns>
        public static Func<int, int> AddPartial(int a, int b) => (c) => a + b + c;
    }
}
