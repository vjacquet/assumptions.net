using System.Numerics;
using Xunit;

namespace NetCore.Assumptions.Numerics
{
    public class AboutVectors
    {
        [Fact]
        public void Assume_Vector_Count_is_constant()
        {
            Assert.Multiple(new[] {
                () => Assert.Equal(4, Vector<double>.Count),
                () => Assert.Equal(4, Vector<long>.Count),
                () => Assert.Equal(8, Vector<int>.Count),
            });
        }

    }
}
