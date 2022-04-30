using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NetCore.Assumptions
{
    public class AboutQueryable
    {
        [Fact]
        public void AssumeQueryableIsPropagated()
        {
            int[] array = new[] { 1, 2, 3, 4, 5, 6 };
            var queryable = array.AsQueryable();
            var query = from i in queryable
                        where i % 2 == 0
                        select i;

            Assert.IsAssignableFrom<IEnumerable<int>>(query);
            Assert.IsAssignableFrom<IQueryable<int>>(query);
        }
    }
}
