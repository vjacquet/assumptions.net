using System;
using Xunit;
using Xunit.Abstractions;

namespace NetCore.Assumptions
{
    public class AboutEnums
    {
        private readonly ITestOutputHelper output;

        public AboutEnums(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Assume_Parse_throws_on_null()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Enum.Parse<DayOfWeek>(null));
            output.WriteLine(ex.Message);
        }

        [Fact]
        public void Assume_Parse_throws_on_empty()
        {
            var ex = Assert.Throws<ArgumentException>(() => Enum.Parse<DayOfWeek>(""));
            output.WriteLine(ex.Message);
        }

        [Fact]
        public void Assume_Parse_throws_on_invalid()
        {
            var ex = Assert.Throws<ArgumentException>(() => Enum.Parse<DayOfWeek>("January"));
            output.WriteLine(ex.Message);
        }

        [Fact]
        public void Assume_Parse_throws_on_invalid_format()
        {
            var ex = Assert.Throws<FormatException>(() => DayOfWeek.Tuesday.ToString("Z"));
            output.WriteLine(ex.Message);
        }

        [Fact]
        public void Assume_Parse_throws_on_Span_created_on_null()
        {
            string value = null;
            var ex = Assert.Throws<ArgumentException>(() => Enum.Parse<DayOfWeek>(value.AsSpan()));
            output.WriteLine(ex.Message);
        }

        [Fact]
        public void Assume_names_in_the_order_of_values()
        {
            var expected = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine" };
            var actual = Enum.GetNames<Digit>();
            Assert.Equal(expected, actual);
        }

        public enum Digit
        {
            One = 1,
            Two,
            Three,
            Four,
            Five,
            Six,
            Seven,
            Eight,
            Nine,
            Zero = 0
        }
    }
}
