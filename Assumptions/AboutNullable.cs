using System;
using System.Collections.Generic;
using Xunit;

namespace NetCore.Assumptions
{
    public class AboutNullable
    {
        [Fact]
        public void NullableInvokeDoesNotEvaluateAguments()
        {
            var counter = new Counter();

            Action<int> action = null;
            action?.Invoke(counter.Increment());

            Assert.Equal(0, counter.Tally);
        }

        [Fact]
        public void NullIsAbsorbantForArithmetics()
        {
            int i = 1;
            int? j = null;
            var result = i + j;
            Assert.Null(result);
        }

        [Fact]
        public void FallbackHasLessPrecedenceThanAdd()
        {
            int a = 10;
            int b = 20;
            int? c = null;
            var result = a + c ?? b;
            Assert.Equal(20, result);
        }

        [Fact]
        public void ComparisonOperatorsWithNullAlwaysReturnsFalse()
        {
            int? a = null;
            int? b = 5;
            int? c = null;

            Assert.False(a < b);
            Assert.False(a <= b);
            Assert.False(a > b);
            Assert.False(a >= b);

            Assert.False(b < a);
            Assert.False(b <= a);
            Assert.False(b > a);
            Assert.False(b >= a);

            Assert.False(a < c);
            Assert.False(a <= c);
            Assert.False(a > c);
            Assert.False(a >= c);
        }

        [Fact]
        public void ComparingWithNullAlwaysReturnsFalse()
        {
            int? a = null;
            int? b = 5;
            int? c = null;

            var comparer = Comparer<int?>.Default;

            Assert.True(comparer.Compare(a, b) < 0);
            Assert.True(comparer.Compare(a, b) <= 0);
            Assert.False(comparer.Compare(a, b) > 0);
            Assert.False(comparer.Compare(a, b) >= 0);

            Assert.False(comparer.Compare(b, a) < 0);
            Assert.False(comparer.Compare(b, a) <= 0);
            Assert.True(comparer.Compare(b, a) > 0);
            Assert.True(comparer.Compare(b, a) >= 0);

            Assert.False(comparer.Compare(a, c) < 0);
            Assert.True(comparer.Compare(a, c) <= 0);
            Assert.False(comparer.Compare(a, c) > 0);
            Assert.True(comparer.Compare(a, c) >= 0);
        }

        [Fact]
        public void EquallingWithNullDoesAlwaysReturnsFalse()
        {
            int? a = null;
            int? b = 5;
            int? c = null;

            Assert.False(a == b, "a == b");
            Assert.True(a != b, "a != b");

            Assert.True(a == c, "a == c");
            Assert.False(a != c, "a != c");
        }
    }
}
