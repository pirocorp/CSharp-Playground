namespace Demo
{
    using System;

    public static class Program
    {
        public static void Main()
        {
            var str = new RepeatSequence();

            for (var i = 0; i < 10; i++)
            {
                Console.WriteLine(str++);
            }
        }

        public static double MidPoint(double left, double right)
            => (left + right) / (2.0);

        public static T MidPoint<T>(T left, T right) where T : INumber<T>
            => left / (T.One + T.One) + right / (T.One + T.One);

        // The constraint that the type argument, T implements IGetNext<T>
        // ensures that the signature for the operator includes the containing type,
        // or its type argument. **Many operators enforce that its parameters
        // must match the type**, or be the type parameter constrained to implement
        // the containing type. Without this constraint, the ++ operator
        // couldn't be defined in the IGetNext<T> interface.
        public interface IGetNext<T> where T : IGetNext<T>
        {
            static abstract T operator ++(T other);
        }

        // with operator can work only with record type, structure type and anonymous type
        // with expression produces a copy of its operand with the specified properties and fields modified
        public struct RepeatSequence : IGetNext<RepeatSequence>
        {
            private const char Character = 'A';

            private string Text = new(Character, 1);

            public RepeatSequence()
            {
            }

            public static RepeatSequence operator ++(RepeatSequence other)
                => other with { Text = other.Text + Character };

            public override string ToString() => this.Text;
        }
    }
}
