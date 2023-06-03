using Xunit;

namespace NetCore.Assumptions
{
    public class AboutPatternMatching
    {
        [Fact]
        public void Is_Object_is_true_on_valid_String()
        {
            string s = "Hello world";
            Assert.True(s is object);
        }

        [Fact]
        public void Is_Object_Is_false_On_Null_String()
        {
            string s = null;
            Assert.False(s is object);
        }

        [Fact]
        public void Is_String_is_true_on_valid_String()
        {
            string s = "Hello world";
            Assert.True(s is string);
        }

        [Fact]
        public void Is_String_is_false_on_Null_String()
        {
            string s = null;
            Assert.False(s is string);
        }
    }
}
