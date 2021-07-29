using System;
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
    }
}
