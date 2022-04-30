using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NetCore.Assumptions
{
    public class AboutClasses
    {
        [Fact]
        public void Overloaded_Foo_hides_virtual_Foo()
        {
            Derived d = new Derived();
            int i = 10;
            var actual = d.Foo(i);
            var expected = "Derived.Foo(object)";
            Assert.Equal(expected, actual);
        }

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
    }
}
