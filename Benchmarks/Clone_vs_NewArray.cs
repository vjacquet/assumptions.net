using BenchmarkDotNet.Attributes;

namespace Benchmarks
{
    public class Clone_vs_NewArray
    {
        static readonly int[] _data = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 };

        [Benchmark(Baseline = true)]
        public int[] NewArray()
        {
            return new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 };
        }

        [Benchmark]
        public int[] Clone()
        {
            return (int[])_data.Clone();
        }
    }
}
