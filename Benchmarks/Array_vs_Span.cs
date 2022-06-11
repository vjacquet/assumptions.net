using BenchmarkDotNet.Attributes;

namespace Benchmarks
{
    public class Array_vs_Span
    {
        [ParamsSource(nameof(GenerateBuffers))]
        public byte[] Buffer { get; set; } = Array.Empty<byte>();

        [Params(Section.All, Section.Half)]
        public Section Strategy { get; set; }

        [Benchmark(Baseline = true)]
        public byte[] ClearArray()
        {
            switch (Strategy)
            {
                case Section.All:
                    Array.Clear(Buffer);
                    break;
                case Section.Half:
                    Array.Clear(Buffer, 0, Buffer.Length / 2);
                    break;
            }
            return Buffer;
        }

        [Benchmark]
        public byte[] ClearSpan()
        {
            switch (Strategy)
            {
                case Section.All:
                    Buffer.AsSpan().Clear();
                    break;
                case Section.Half:
                    Buffer.AsSpan(0, Buffer.Length / 2).Clear();
                    break;
            }
            return Buffer;
        }

        #region Generators

        public static IEnumerable<byte[]> GenerateBuffers()
        {
            var random = new Random(1789);
            var sizes = new[] { 256, 512, 1024, 2048, 4096, 8192 };
            foreach (var size in sizes)
            {
                var buffer = new byte[size];
                random.NextBytes(buffer);
                yield return buffer;
            }
        }

        public enum Section
        {
            All,
            Half
        }

        #endregion
    }
}
