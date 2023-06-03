using Xunit;
using Xunit.Abstractions;

namespace NetCore.Assumptions.Reflection
{
    public class AboutIL
    {
        private readonly ITestOutputHelper output;

        public AboutIL(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory]
        [InlineData("IfStatement")]
        [InlineData("ConditionalExpression")]
        public void Null_Coalesce_Yield_Smaller_IL_Code_Than(string method)
        {
            var type = GetType();
            var baseLine = type.GetMethod("NullCoalesce").GetMethodBody();
            var target = type.GetMethod(method).GetMethodBody();

            var baseLineIL = baseLine.GetILAsByteArray();
            var targetIL = target.GetILAsByteArray();

            output.WriteLine($"NullCoalesce maxstack {baseLine.MaxStackSize}, locals {baseLine.LocalVariables.Count},  code size {baseLineIL.Length}");
            output.WriteLine($"{method} maxstack {target.MaxStackSize}, locals {target.LocalVariables.Count},  code size {targetIL.Length}");
            Assert.True(baseLineIL.Length < targetIL.Length);
        }

        public static string IfStatement(string x)
        {
            if (x == null)
                return "Empty";
            return x;
        }

        public static string ConditionalExpression(string x)
        {
#pragma warning disable IDE0029 // Use coalesce expression
            return x == null ? "Empty" : x;
#pragma warning restore IDE0029 // Use coalesce expression
        }

        public static string NullCoalesce(string x)
        {
            return x ?? "Empty";
        }
    }
}
