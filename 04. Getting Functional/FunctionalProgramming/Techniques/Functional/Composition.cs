namespace Functional
{
    using System;

    public static partial class Techniques
    {
        public static Func<TA, TC> Compose<TA, TB, TC>(Func<TA, TB> funcA, Func<TB, TC> funcB)
            => x => funcB(funcA(x));

        public static Func<TA, TD> Compose<TA, TB, TC, TD>(Func<TA, TB> funcA, Func<TB, TC> funcB, Func<TC, TD> funcC)
            => x => funcC(funcB(funcA(x)));
    }
}
