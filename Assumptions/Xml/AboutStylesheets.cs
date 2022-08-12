using System.Text;
using System.Xml;
using System.Xml.Xsl;
using Xunit;
using Xunit.Abstractions;

namespace NetCore.Assumptions.Xml
{
    public class AboutStylesheets
    {
        private readonly ITestOutputHelper output;

        public AboutStylesheets(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void XslCompiledTransform_processes_literal_result_element()
        {
            var document = new XmlDocument();
            document.LoadXml(@"<expense-report><total>100</total></expense-report>");

            var literal = new XmlDocument();
            literal.LoadXml(@"<html xsl:version=""1.0"" xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns=""http://www.w3.org/TR/xhtml1/strict"">
  <head>
    <title>Expense Report Summary</title>
  </head>
  <body>
    <p>Total Amount: <xsl:value-of select=""expense-report/total""/></p>
  </body>
</html>");
            var stylesheet = new XslCompiledTransform();
            stylesheet.Load(literal);

            var sb = new StringBuilder();
            using (var writer = XmlWriter.Create(sb))
                stylesheet.Transform(document, writer);

            var result = sb.ToString();
            Assert.NotNull(result);
            output.WriteLine(result);
        }
    }
}
