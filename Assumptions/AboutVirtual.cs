using Xunit;

namespace NetCore.Assumptions
{
    public class AboutVirtual
    {
        [Fact]
        public void AssumeOverloadHidesOverride()
        {
            var d = new Derived();
            var actual = d.Foo(10);
            Assert.Equal("Derived.Foo(object)", actual);
        }


        #region Suite

        class Base
        {
            public virtual string Foo(int x)
            {
                return "Base.Foo(int)";
            }
        }

        class Derived : Base
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
