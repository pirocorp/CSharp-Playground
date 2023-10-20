namespace CSharp11Demo;

using System;

using CSharp11Demo.AbstractingOverStaticMembers;
using CSharp11Demo.GenericAttributes;
using CSharp11Demo.RequiredMembers;

public static class Program
{
    public static void Main()
    {
        Console.WriteLine($"C# 11 Demos:");
        RawStringLiterals();
        AbstractingOverStaticMembers();
        ListPatterns();
        RequiredMembers();
        GenericAttributes();
    }

    private static void RawStringLiterals()
    {
        PrintMessage("Raw string literals");

        var raw1 = """This\is\all "content"!""";
        var raw2 = """""I can do ", "", """ or even """" double quotes!""""";
        var raw3 = """
            <element attr="content">
            <body>
            This line is indented by 4 spaces.
            </body>
            </element>
            """;

        Console.WriteLine(raw1);
        Console.WriteLine(raw2);
        Console.WriteLine(raw3);
    }

    private static void AbstractingOverStaticMembers()
    {
        PrintMessage("Abstracting over static members");

        var pointA = new Point(3, 4);
        var pointB = new Point(4, -3);

        var myIntA = new MyInt(5);
        var myIntB = new MyInt(6);

        var myIntC = myIntA + myIntB;
        Console.WriteLine($"MyInt: {myIntC}");

        var pointC = pointA + pointB;
        Console.WriteLine($"Point: {pointC}");

        var sum = AddAll(
            new MyInt(3),
            new MyInt(4),
            new MyInt(5)
        );

        Console.WriteLine($"Sum: {sum}");
    }

    private static T AddAll<T>(params T[] elements) where T : IMonoid<T>
    {
        var result = T.Zero;

        foreach (var element in elements)
        {
            result += element;
        }

        return result;
    }

    private static void ListPatterns()
    {
        PrintMessage("List patterns");

        var sum = AddAllPattern(
            new MyInt(3),
            new MyInt(4),
            new MyInt(5),
            new MyInt(6),
            new MyInt(7)
        );

        Console.WriteLine($"Sum: {sum}");
    }

    private static T AddAllPattern<T>(params T[] elements) where T : IMonoid<T> 
        => elements switch
        {
            [] => T.Zero,
            [var first, .. var rest] => first + AddAll(rest),
        };

    private static void RequiredMembers()
    {
        PrintMessage("Required Members");

        // It is an error to create a Person without initializing
        // both the required properties
        var person = new Person { FirstName = "Ada", LastName = "Pens" };
        Console.WriteLine(person);
    }

    [Generic<string>]
    private static void GenericAttributes()
    {
        PrintMessage("Generic Attributes");
    }

    private static void PrintMessage(string message)
    {
        var spacer = new string('-', 20);
        Console.WriteLine();
        Console.WriteLine($"{spacer}{message}{spacer}");
    }
}
