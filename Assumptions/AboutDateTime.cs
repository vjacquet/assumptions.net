using System;
using Xunit;

namespace NetCore.Assumptions
{
    public class AboutDateTime
    {
        [Fact]
        public void DateTime_ignore_kind_when_comparing_equal()
        {
            var unspecified = new DateTime(1999, 12, 31, 23, 59, 59, DateTimeKind.Unspecified);
            var utc = new DateTime(1999, 12, 31, 23, 59, 59, DateTimeKind.Utc);
            var local = new DateTime(1999, 12, 31, 23, 59, 59, DateTimeKind.Local);

            Assert.Multiple(new Action[] {
                () => Assert.Equal(unspecified, utc),
                () => Assert.Equal(unspecified, local),
                () => Assert.Equal(utc, local),

                () => Assert.True(unspecified == utc),
                () => Assert.True(unspecified == local),
                () => Assert.True(utc == local),
            });
        }

        [Fact]
        public void DateTime_ignore_kind_when_comparing()
        {
            var unspecified = new DateTime(1999, 12, 31, 23, 59, 59, DateTimeKind.Unspecified);
            var utc = new DateTime(1999, 12, 31, 23, 59, 59, DateTimeKind.Utc);
            var local = new DateTime(1999, 12, 31, 23, 59, 59, DateTimeKind.Local);

            Assert.Multiple(new Action[] {
                () => Assert.Equal(0, unspecified.CompareTo(utc)),
                () => Assert.Equal(0, unspecified.CompareTo(local)),
                () => Assert.Equal(0, utc.CompareTo(local)),

                () => Assert.True(unspecified <= utc),
                () => Assert.True(unspecified <= local),
                () => Assert.True(utc <= local),
            });
        }
    }
}
