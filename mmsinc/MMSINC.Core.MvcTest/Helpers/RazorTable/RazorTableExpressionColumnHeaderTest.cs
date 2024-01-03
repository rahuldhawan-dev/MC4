using System.ComponentModel;
using System.Web.Mvc;
using MMSINC.Data;
using MMSINC.Helpers;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Container = StructureMap.Container;

namespace MMSINC.Core.MvcTest.Helpers.RazorTable
{
    [TestClass]
    public class RazorTableExpressionColumnHeaderTest
    {
        #region Fields

        private HtmlHelper<Model> _htmlHelper;
        private Mock<IRazorTableColumn<Model>> _column;

        #endregion

        #region Init

        [TestInitialize]
        public void InitializeTest()
        {
            _column = new Mock<IRazorTableColumn<Model>>();
            _htmlHelper = new FakeMvcHttpHandler(new Container()).CreateHtmlHelper<Model>();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestRenderTagWithExpressionPropertyNameAsText()
        {
            var result = new RazorTableExpressionColumnHeader<Model, string>(x => x.Name)
                        .Render(_htmlHelper, _column.Object, null).ToString();
            Assert.AreEqual("<th data-property=\"Name\">Name</th>", result);
        }

        [TestMethod]
        public void TestRendersTagWithPrettyFormattedText()
        {
            var result = new RazorTableExpressionColumnHeader<Model, string>(x => x.AnotherProperty)
                        .Render(_htmlHelper, _column.Object, null).ToString();
            Assert.AreEqual("<th data-property=\"AnotherProperty\">Another Property</th>", result);
        }

        [TestMethod]
        public void TestRendersTagWithHtmlEscapedFromText()
        {
            var result = new RazorTableExpressionColumnHeader<Model, string>(x => x.AnUglyProperty)
                        .Render(_htmlHelper, _column.Object, null).ToString();
            Assert.AreEqual("<th data-property=\"AnUglyProperty\">R &amp; D</th>", result);
        }

        [TestMethod]
        public void TestConstructorThrowsExceptionIfInvalidExpression()
        {
            MyAssert.Throws(() => new RazorTableExpressionColumnHeader<Model, Model>(x => x));
        }

        [TestMethod]
        public void TestConstructorThrowsForNullExpressionParameter()
        {
            MyAssert.Throws(() => new RazorTableExpressionColumnHeader<Model, Model>(null));
        }

        #endregion

        #region Helper classes

        public class Model
        {
            public string Name { get; set; }

            public string AnotherProperty { get; set; }

            [DisplayName("R & D")]
            public string AnUglyProperty { get; set; }
        }

        #endregion
    }
}
