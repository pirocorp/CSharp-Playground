namespace FastIterations;

using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;

[MemoryDiagnoser(false)]
public class Benchmarks
{
    private int[] items;

    public Benchmarks()
    {
        this.items = Array.Empty<int>();
    }
    
    [Params(100, 100_000, 1_000_000)]
    public int Size { get; set; }

    public void Setup()
    {
        var random = new Random(42);

        this.items = Enumerable
            .Range(1, this.Size)
            .Select(x => random.Next())
            .ToArray();
    }

    [Benchmark]
    public int[] For()
    {
        for (var i = 0; i < this.items.Length; i++)
        {
            var item = this.items[i];

            DoSomething(item);
        }

        return this.items;
    }

    [Benchmark]
    public int[] Foreach()
    {
        foreach (var item in items)
        {
            DoSomething(item);
        }

        return this.items;
    }

    [Benchmark]
    public int[] For_Span()
    {
        Span<int> asSpan = this.items;

        for (var i = 0; i < asSpan.Length; i++)
        {
            var item = asSpan[i];
            DoSomething(item);
        }

        return this.items;
    }

    [Benchmark]
    public int[] Unsafe_Memory()
    {
        ref var searchSpace = ref MemoryMarshal.GetArrayDataReference(items);

        for (var i = 0; i < items.Length; i++)
        {
            var item = Unsafe.Add(ref searchSpace, i);

            DoSomething(item);
        }

        return this.items;
    }

    private static void DoSomething(int item)
    {

    }
}
