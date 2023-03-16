using BenchmarkDotNet.Attributes;
using Benchmarks.Internals;

namespace Benchmarks
{
	public class ctor_vs_piecewise
	{
		[Benchmark(Baseline = true)]
		public Record UseConstructor()
		{
			return new Record("Lorem ipsum dolor sit amet");
		}


		[Benchmark]
		public Record UsePiecewiseConstructor()
		{
			return new Record("Lorem ipsum dolor sit amet", default);
		}

	}
}
namespace Benchmarks.Internals
{
	internal struct Piecewise
	{
	}

	public class Record
	{
		private readonly string storage;

		public Record(string storage)
		{
			this.storage = storage;
		}

		internal Record(string storage, Piecewise _)
		{
			this.storage = storage;
		}
	}

}