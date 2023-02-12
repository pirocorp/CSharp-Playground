namespace FastIterations;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

public static class IteratingWithLowLevelMemoryAccess
{
    private static readonly IEnumerable<int> Numbers;

    static IteratingWithLowLevelMemoryAccess()
    {
        var random = new Random(42);

        Numbers = Enumerable
            .Range(0, 20)
            .Select(x => random.Next());
    }

    public static void ClassIteration()
    {
        var items = Numbers
            .Select(x => new Wrapper() { Number = x, Text = x.ToString() })
            .ToList();

        IterateMemory(items);
    }

    public static void RecordIteration()
    {
        var items = Numbers
            .Select(x => new WrapperRecord(x, x.ToString()))
            .ToList();

        Iterate(items);
    }

    private static void Iterate<T>(IEnumerable<T> items)
    {
        var listAsSpan = CollectionsMarshal.AsSpan(items.ToList());
        ref var searchSpace = ref MemoryMarshal.GetReference(listAsSpan);

        for (var i = 0; i < listAsSpan.Length; i++)
        {
            var item = Unsafe.Add(ref searchSpace, i);

            Console.WriteLine(item);
        }
    }

    private static void IterateMemory<T>(IEnumerable<T> items)
    {
        var array = items.ToArray();

        ref var searchSpace = ref MemoryMarshal.GetArrayDataReference(array);

        for (var i = 0; i < array.Length; i++)
        {
            var item = Unsafe.Add(ref searchSpace, i);

            Console.WriteLine(item);
        }
    }
}

internal class Wrapper
{
    public int Number { get; set; }

    public string Text { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"(Number: {this.Number}, Text: {this.Text})";
    }
}

internal record WrapperRecord(int Number, string Text);
