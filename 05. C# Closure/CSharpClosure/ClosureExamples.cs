namespace CSharpClosure
{
    using System;

    /// <summary>
    /// In computer science, a closure is a first-class function with free variables
    /// that are bound in the lexical environment.
    /// </summary>
    /// <remarks>
    /// The C# compiler detects when a delegate forms a closure which is passed
    /// out of the current scope and it promotes the delegate, and the associated local
    /// variables into a compiler generated class.
    /// </remarks>
    public static class ClosureExamples
    {
        public static Func<int> Incrementator()
        {
            var count = 0;

            return () => count++;
        }
    }
}
