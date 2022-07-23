using System.Collections.Generic;
using Xunit;

namespace NetCore.Assumptions
{
    public class AboutDefaultDoubleComparer
    {
        private static readonly EqualityComparer<double> Comparer = EqualityComparer<double>.Default;

        [Theory]
        [InlineData(0.0)]
        [InlineData(1.0)]
        [InlineData(double.NaN)]
        [InlineData(double.PositiveInfinity)]
        [InlineData(double.NegativeInfinity)]
        public void Equals_is_reflexive(double a)
        {
            Assert.True(Comparer.Equals(a, a));
        }

        [Theory]
        [InlineData(0.0, 1.0)]
        [InlineData(0.0, double.NaN)]
        [InlineData(1.0, double.NaN)]
        [InlineData(0.0, double.PositiveInfinity)]
        [InlineData(1.0, double.PositiveInfinity)]
        [InlineData(0.0, double.NegativeInfinity)]
        [InlineData(1.0, double.NegativeInfinity)]
        [InlineData(double.NaN, double.PositiveInfinity)]
        [InlineData(double.NaN, double.NegativeInfinity)]
        public void Equals_is_symmetric(double a, double b)
        {
            Assert.Equal(Comparer.Equals(a, b), Comparer.Equals(b, a));
        }
    }
}
