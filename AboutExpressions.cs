using System;
using System.Linq.Expressions;
using Xunit;

namespace NetCore.Assumptions
{
    public class AboutExpressions
    {
        [Fact]
        public void CompilingTwiceGeneratesDifferentDelegates()
        {
            Expression<Func<int, bool>> fn = x => x > 10;

            var f1 = fn.Compile();
            var f2 = fn.Compile();
            Assert.False(ReferenceEquals(f1, f2));
        }
    }
}
