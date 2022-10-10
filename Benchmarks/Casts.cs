using BenchmarkDotNet.Attributes;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Benchmarks
{
    public class Casts
    {
        public static List<Party> People = new List<Party>(10000);

        static Casts()
        {
            for (int i = 0; i < People.Capacity; i++)
            {
                var id = Guid.NewGuid();
                People.Add(new Person { Id = id, FullName = id.ToString() });
            }
        }

        [Benchmark(Baseline = true)]
        public List<Person> OfType()
        {
            return ((IList)People).OfType<Person>().ToList();
        }

        [Benchmark]
        public List<Person> CustomObjectOfType()
        {
            return People.OfType<Person>().ToList();
        }

        [Benchmark]
        public List<Person> IsThenCast()
        {
            return Enumerable.Cast<Person>(People.Where(x => x is Person)).ToList();
        }


        [Benchmark]
        public List<Person> IsThenCustomCast()
        {
            return People.Where(x => x is Person).Cast<Person>().ToList();
        }

        [Benchmark]
        public List<Person> IsThenHardCast()
        {
            return People.Where(x => x is Person).Select(x => (Person)x).ToList();
        }


        [Benchmark]
        public List<Person> GetTypeThenHardCast()
        {
            return People.Where(x => x.GetType() == typeof(Person)).Select(x => (Person)x).ToList();
        }

        [Benchmark]
        public List<Person> IsAssignableToThenHardCast()
        {
            return People.Where(x => x.GetType().IsAssignableTo(typeof(Person))).Select(x => (Person)x).ToList();
        }

        public class Party
        {
            public Guid Id { get; set; }
        }
        public class Person : Party
        {
            public string FullName { get; set; } = default!;
        }
    }

    public static class CustomCastExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> Cast<T>(this IEnumerable<object> source)
        {
            return source.Select(x => (T)x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> OfType<T>(this IEnumerable<object> source)
        {
            return source.Where(x => x is T).Select(x => (T)x);
        }

        public static IEnumerable<T> CustomCast<T>(this IEnumerable source)
            where T : class
        {
            if (source is IEnumerable<object> enumerable)
                return Cast<T>(enumerable);

            return new CastIterator<T>(source);
        }

        internal class CastIterator<T> : IEnumerable<T>, IEnumerator<T>
            where T : class
        {
            private readonly int _threadId;
            internal int _state;
            internal T _current = default!;

            private readonly IEnumerable _source;
            private IEnumerator? _enumerator;

            public CastIterator(IEnumerable source)
            {
                _threadId = Environment.CurrentManagedThreadId;
                Debug.Assert(source != null);
                _source = source;
            }

            public T Current => _current;

            public void Dispose()
            {
                if (_enumerator is IDisposable disposable)
                {
                    disposable.Dispose();
                }
                _enumerator = null;

                _current = default!;
                _state = -1;
            }

            protected CastIterator<T> Clone() =>
               new CastIterator<T>(_source);

            public IEnumerator<T> GetEnumerator()
            {
                CastIterator<T> enumerator = _state == 0 && _threadId == Environment.CurrentManagedThreadId ? this : Clone();
                enumerator._state = 1;
                return enumerator;
            }

            public bool MoveNext()
            {
                switch (_state)
                {
                    case 1:
                        _enumerator = _source.GetEnumerator();
                        _state = 2;
                        goto case 2;
                    case 2:
                        Debug.Assert(_enumerator != null);
                        if (_enumerator.MoveNext())
                        {
                            _current = (T)_enumerator.Current;
                            return true;
                        }

                        Dispose();
                        break;
                }

                return false;
            }

            object? IEnumerator.Current => Current;

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            void IEnumerator.Reset() => throw new NotSupportedException();
        }
    }
}
