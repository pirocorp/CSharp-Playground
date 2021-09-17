namespace CSharpFunctionComposition
{
    using System;

    public static class FunctionCompositionExample
    {
        public static Func<TIn1, TOut> Compose<TIn1, TIn2, TOut>(
            Func<TIn1, TIn2> f1,
            Func<TIn2, TOut> f2)
        {
            return x => f2(f1(x));
        }
    }
}
