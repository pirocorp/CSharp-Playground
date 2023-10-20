namespace CSharp11Demo.Infrastructure
{
    using System.Diagnostics.CodeAnalysis;
    using System;

    public static class Validators
    {
        public static T CheckNotNull<T>([NotNull] T? input) where T : class 
            => input ?? throw new ArgumentNullException();
    }
}
