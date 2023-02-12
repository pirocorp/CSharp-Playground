namespace FastIterations
{
    using System;
    using BenchmarkDotNet.Running;

    public static class Program
    {
        public static void Main()
        {
            IteratingWithLowLevelMemoryAccess.RecordIteration();
            IteratingWithLowLevelMemoryAccess.ClassIteration();

            var summary = BenchmarkRunner.Run<Benchmarks>();

            Console.WriteLine(summary.BenchmarksCases);
        }
    }
}
