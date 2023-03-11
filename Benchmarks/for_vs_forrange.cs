using BenchmarkDotNet.Attributes;
using System.Runtime.CompilerServices;
using Benchmarks.Internals;
using BenchmarkDotNet.Jobs;

namespace Benchmarks
{
	[SimpleJob(RuntimeMoniker.Net60)]
	[SimpleJob(RuntimeMoniker.Net70)]
	[RPlotExporter]
	public class for_vs_forrange
	{
		[Params(10, 100, 1000, 10_000)]
		public int Size { get; set; }

		[Benchmark(Baseline = true)]
		public void ForLoop()
		{
			for (int i = 0; i < Size; i++)
			{
				DoSomething(i);
			}
		}


		[Benchmark]
		public void ForeachRangeLoop()
		{
			foreach (int i in 0..Size)
			{
				DoSomething(i);
			}
		}

		[MethodImpl(MethodImplOptions.NoOptimization)]
		private static void DoSomething(int _)
		{
		}
	}
}

namespace Benchmarks.Internals
{
	internal static class RangeExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Enumerator GetEnumerator(this Range range)
		{
			return new Enumerator(range);
		}

		public ref struct Enumerator
		{
			private int _current;
			private int _start;
			private readonly int _end;
			private readonly int _stride;

			public Enumerator(Range range)
			{
				_current = default;
				_start = range.Start.GetOffset(int.MaxValue);
				_end = range.End.GetOffset(int.MaxValue);
				_stride = _start < _end ? +1 : -1;
			}

			public int Current => _current;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool MoveNext()
			{
				if (_start != _end)
				{
					_current = _start;
					_start += _stride;
					return _current != _end;
				}
				return false;
			}
		}
	}

}