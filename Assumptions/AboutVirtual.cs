using Xunit;

namespace NetCore.Assumptions
{
    public class AboutVirtual
    {
        [Fact]
        public void Assume_overload_hides_override()
        {
            var d = new Derived();
            var actual = d.Foo(10);
            Assert.Equal("Derived.Foo(object)", actual);
        }

        #region Suite

        private class Base
        {
            public virtual string Foo(int x)
            {
                return "Base.Foo(int)";
            }
        }

        private class Derived : Base
        {
            public override string Foo(int x)
            {
                return "Derived.Foo(int)";
            }

            public string Foo(object o)
            {
                return "Derived.Foo(object)";
            }
        }

        #endregion
    }
}
