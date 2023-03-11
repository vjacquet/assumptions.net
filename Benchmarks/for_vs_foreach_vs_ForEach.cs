using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System.Runtime.InteropServices;

namespace Benchmarks
{
	[SimpleJob(RuntimeMoniker.Net60)]
	[SimpleJob(RuntimeMoniker.Net70)]
	[RPlotExporter]
	public class for_vs_foreach_vs_ForEach
	{
		private List<int>? _items;

		[GlobalSetup]
		public void Setup()
		{
			var random = new Random(420);
			_items = new List<int>(Size);
			_items.AddRange(Enumerable.Range(1, Size).Select(_ => random.Next()));
		}

		[Params(100, 100_000)] public int Size { get; set; }

		[Benchmark(Baseline = true)]
		public void Foreach()
		{
			foreach (var item in _items)
			{
				Noop(item);
			}
		}

		[Benchmark]
		public void For()
		{
			for (var i = 0; i < _items.Count; i++)
			{
				Noop(_items[i]);
			}
		}

		[Benchmark]
		public void ForeachLinq()
		{
			_items.ForEach(Noop);
		}

		[Benchmark]
		public void ParallelForeachLinq()
		{
			Parallel.ForEach(_items, Noop);
		}

		[Benchmark]
		public void Span()
		{
			foreach (var item in CollectionsMarshal.AsSpan(_items))
			{
				Noop(item);
			}
		}

		private static void Noop(int value)
		{
		}
	}
}
