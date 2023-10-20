namespace CSharp11Demo.AbstractingOverStaticMembers;

/// <summary>
/// How do you abstract over operations that are inherently static – such as operators?
/// The traditional answer is “poorly”.
/// In C# 11 we released support for static virtual members in interfaces,
/// which was in preview in C# 10. 
/// </summary>
public interface IMonoid<TSelf> where TSelf : IMonoid<TSelf>
{
    public static abstract TSelf operator +(TSelf a, TSelf b);

    public static abstract TSelf Zero { get; }
}
