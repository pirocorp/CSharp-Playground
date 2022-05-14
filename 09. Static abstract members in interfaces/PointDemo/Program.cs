namespace PointDemo
{
    using System;

    public static class Program
    {
        public static void Main()
        {
            var pt = new Point<int>(3, 4);

            var translate = new Translation<int>(5, 10);

            var final = pt + translate;

            Console.WriteLine(pt);
            Console.WriteLine(translate);
            Console.WriteLine(final);
        }
    }

    // It's useful to have a property that defines the additive identity (an identity element
    // (such as 0 in the group of whole numbers under the operation of addition)
    // that in a given mathematical system leaves unchanged any element to which it is added) value for the type.
    // There's a new experimental interface for that feature: IAdditiveIdentity<TSelf, TResult>.
    // A translation of {0, 0} is the additive identity:
    public record Translation<T>(T XOffset, T YOffset) : IAdditiveIdentity<Translation<T>, Translation<T>>
        where T : IAdditiveIdentity<T, T>
    {
        public static Translation<T> AdditiveIdentity => new(XOffset: T.AdditiveIdentity, YOffset: T.AdditiveIdentity);
    }
    
    // IAdditionOperators<Point<T>, Translation<T>, Point<T>> interface. The Point type makes use of different types for operands and the result.
    public record Point<T>(T X, T Y) : IAdditionOperators<Point<T>, Translation<T>, Point<T>>
        where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T> // Declare that T supports the IAdditionOperators<TSelf, TOther, TResult> interface. That interface includes the operator + static method.
    {
        public static Point<T> operator +(Point<T> left, Translation<T> right) 
            => new(X: left.X + right.XOffset, Y: left.Y + right.YOffset);
    }
}
