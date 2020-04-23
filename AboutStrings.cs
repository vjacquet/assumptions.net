using System;
using Xunit;

namespace NetCore.Assumptions
{
    public class AboutStrings
    {
        [Theory]
        [InlineData(false, "a", "b")]
        [InlineData(false, "a", null)]
        [InlineData(false, null, "b")]
        [InlineData(true, null, null)]
        [InlineData(true, "a", "a")]
        public void StringComparerAcceptsNull(bool expected, string a, string b)
        {
            var actual = StringComparer.Ordinal.Equals(a, b);
            Assert.Equal(expected, actual);
        }
    }
}
