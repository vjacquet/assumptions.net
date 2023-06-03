using System;
using Xunit;

namespace NetCore.Assumptions
{
    public class AboutOperators
    {
        [Fact]
        public void Relation_operators_are_not_implicit_on_comparable_structs()
        {
            var q1 = new Quantity(1);
            var q2 = new Quantity(2);
            Assert.True(q1.CompareTo(q2) < 0);
            //Assert.True(q1 < q2);
        }
    }

    public readonly struct Quantity : IEquatable<Quantity>, IComparable<Quantity>
    {
        private readonly decimal storage;

        public Quantity(decimal value)
        {
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(value), value, "The quantity must be positive.");
            storage = value;
        }

        public int CompareTo(Quantity other)
        {
            return storage.CompareTo(other.storage);
        }

        public bool Equals(Quantity other)
        {
            return storage == other.storage;
        }
    }
}
