namespace Functional
{
    using System;

    public static partial class Techniques
    {
        public static Func<TB, TC, TResult> Partial<TA, TB, TC, TResult>(Func<TA, TB, TC, TResult> func, TA parameterA)
            => (TB parameterB, TC parameterC) => func(parameterA, parameterB, parameterC);

        public static Func<TC, TResult> Partial<TA, TB, TC, TResult>(Func<TA, TB, TC, TResult> func, TA parameterA, TB parameterB)
            => (TC parameterC) => func(parameterA, parameterB, parameterC);
    }
}
