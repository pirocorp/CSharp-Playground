namespace CSharp11Demo.AbstractingOverStaticMembers;

public readonly struct MyInt : IMonoid<MyInt>
{
    private readonly int value;

    public MyInt(int value)
    {
        this.value = value;
    }

    public static MyInt operator +(MyInt a, MyInt b)
        => new (a.value + b.value);

    public static MyInt Zero => new (0);

    public override string ToString()
        => this.value.ToString();
}
