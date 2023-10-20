namespace CSharp11Demo.AbstractingOverStaticMembers;

public interface IPoint<TSelf> where TSelf : IPoint<TSelf>
{
    public double X { get; }

    public double Y { get; }

    public static abstract TSelf operator +(TSelf a, TSelf b);
}
