using System;
using System.Web.Optimization;
using MMSINC.Bundling;
using MMSINC.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.MvcTest.Utilities
{
    [TestClass]
    public class DotLessTransformTest
    {
        [TestMethod]
        public void TestProcessUsesDotLessParserOnContent()
        {
            var response = new BundleResponse {
                Content = "@someVariable: #222222; .content { color: @someVariable; }"
            };

            var target = new DotLessTransform();

            target.Process(null, response);

            // The dotLess parser has its own way of formatting the output CSS with
            // line breaks and indentation and what not. Rather than try to find that,
            // doing partial string tests will work fine.
            Console.WriteLine(response.Content);

            Assert.AreNotEqual(string.Empty, response.Content, "The parser probably failed.");
            Assert.IsTrue(response.Content.StartsWith(".content"), "The variable declaration should be gone.");
            Assert.IsTrue(response.Content.Contains("color: #222222;"),
                "The @someVariable variable should be replaced with the actual color.");
        }
    }
}
