namespace Functional
{
    using System;

    public static partial class Techniques
    {
        public static Func<TA, Func<TB, TResult>> Curry<TA, TB, TResult>(Func<TA, TB, TResult> func)
            => (TA parameterA) => (TB parameterB) => func(parameterA, parameterB);

        public static Func<TA, Func<TB, Func<TC, TResult>>> Curry<TA, TB, TC, TResult>(Func<TA, TB, TC, TResult> func)
            => (TA parameterA) => (TB parameterB) => (TC parameterC) => func(parameterA, parameterB, parameterC);
    }
}
