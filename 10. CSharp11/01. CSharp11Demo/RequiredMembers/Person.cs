namespace CSharp11Demo.RequiredMembers
{

    /// <summary>
    /// When creating types that used object initializers,
    /// you used to be unable to specify that some properties
    /// must be initialized. Now, you can say that a property
    /// or field is required. This means that it must be
    /// initialized by an object initializer when an
    /// object of the type is created
    /// </summary>
    public class Person
    {
        public required string FirstName { get; init; }

        public string? MiddleName { get; init; }

        public required string LastName { get; init; }

        public override string ToString()
            => $"{this.FirstName}{(this.MiddleName is null ? " " : $" {this.MiddleName} ")}{this.LastName}";
    }
}
