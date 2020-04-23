using Xunit;

namespace NetCore.Assumptions
{
    public class AboutPatternMatching
    {
        [Fact]
        public void IsObjectIsTrueOnValidString()
        {
            string s = "Hello world";
            Assert.True(s is object);
        }

        [Fact]
        public void IsObjectIsFalseOnNullString()
        {
            string s = null;
            Assert.False(s is object);
        }

        [Fact]
        public void IsStringIsTrueOnValidString()
        {
            string s = "Hello world";
            Assert.True(s is string);
        }

        [Fact]
        public void IsStringIsFalsOnNullString()
        {
            string s = null;
            Assert.False(s is string);
        }
    }
}
