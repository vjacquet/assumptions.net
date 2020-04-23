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
    }
}
