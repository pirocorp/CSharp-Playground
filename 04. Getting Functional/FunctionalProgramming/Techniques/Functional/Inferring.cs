namespace Functional
{
    using System;

    public static partial class Techniques
    {
        public static Func<TA, TResult> Func<TA, TResult>(Func<TA, TResult> func) => func;

        public static Func<TA, TB, TResult> Func<TA, TB, TResult>(Func<TA, TB, TResult> func) => func;

        public static Func<TA, TB, TC, TResult> Func<TA, TB, TC, TResult>(Func<TA, TB, TC, TResult> func) => func;
    }
}
