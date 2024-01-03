using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Helpers;
using MMSINC.Testing;
using MMSINC.Utilities;
using StructureMap;

namespace MMSINC.Core.MvcTest.Helpers.RazorTable
{
    [TestClass]
    public class RazorTableExpressionColumnTest
    {
        #region Fields

        private FakeMvcHttpHandler _request;
        private RazorTableExpressionColumn<Model, string> _target;
        private Model _model;
        private HtmlHelper<Model> _htmlHelper;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _request = new FakeMvcHttpHandler(new Container());
            _target = new RazorTableExpressionColumn<Model, string>(x => x.Name);
            _model = new Model {Name = "Dude"};
            _htmlHelper = _request.CreateHtmlHelper(_model);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestConstructorSetsExpressionProperty()
        {
            Expression<Func<Model, string>> expectedExpression = x => x.Name;
            Assert.AreSame(expectedExpression,
                new RazorTableExpressionColumn<Model, string>(expectedExpression).Expression);
        }

        [TestMethod]
        public void TestConstructorThrowsForNullExpression()
        {
            MyAssert.Throws<ArgumentNullException>(() => new RazorTableExpressionColumn<Model, string>(null));
        }

        [TestMethod]
        public void TestConstructorThrowsForInvalidExpressionWhenExpressionIsInvalid()
        {
            MyAssert.Throws(() => new RazorTableExpressionColumn<Model, Model>(x => x));
        }

        [TestMethod]
        public void TestConstructorSetsPropertyName()
        {
            Assert.AreEqual("Name", new RazorTableExpressionColumn<Model, string>(x => x.Name).PropertyName);
            Assert.AreEqual("AnotherProperty",
                new RazorTableExpressionColumn<Model, object>(x => x.AnotherProperty).PropertyName);
        }

        [TestMethod]
        public void TestRenderCellReturnsTableCellTagWithPlainTextValue()
        {
            var result = _target.RenderCell(_htmlHelper);
            Assert.AreEqual("td", result.TagName);
            Assert.AreEqual(_model.Name, result.InnerHtml);
        }

        [TestMethod]
        public void TestRenderCellHtmlEscapesValues()
        {
            var target = new RazorTableExpressionColumn<Model, object>(x => x.Name);
            var model = new Model {Name = "R & D"};
            var htmlHelper = _request.CreateHtmlHelper(model);
            var result = target.RenderCell(htmlHelper);
            Assert.AreEqual("<td>R &amp; D</td>", result.ToString());
        }

        [TestMethod]
        public void TestRenderCellIncludesDisplayFormatFormatting()
        {
            var target = new RazorTableExpressionColumn<Model, DateTime>(x => x.FormattedDate);
            var model = new Model {FormattedDate = new DateTime(2013, 10, 23)};
            var htmlHelper = _request.CreateHtmlHelper(model);
            var oic =
                htmlHelper.ViewData.ModelMetadata.Properties.Where(x => x.PropertyName == "FormattedDate").Single();
            var result = target.RenderCell(htmlHelper);
            Assert.AreEqual("<td>10/23/2013</td>", result.ToString());
        }

        [TestMethod]
        public void TestRenderHeaderUsesExpressionColumnHeaderByDefault()
        {
            var target = new RazorTableExpressionColumn<Model, object>(x => x.AnotherProperty);
            Assert.IsInstanceOfType(target.Header, typeof(RazorTableExpressionColumnHeader<Model, object>));
            var result = target.RenderColumnHeader(_htmlHelper);
            Assert.AreEqual("<th data-property=\"AnotherProperty\">Another Property</th>", result.ToString());
        }

        [TestMethod]
        public void TestRenderHeaderDoesNotThrowAHissyFitBecauseTheHeaderPropertyGotChanged()
        {
            var target = new RazorTableExpressionColumn<Model, object>(x => x.AnotherProperty);
            target.Header = new RazorTableColumnHeader<Model> {Text = "BilboBaggins"};
            var result = target.RenderColumnHeader(_htmlHelper);
            Assert.AreEqual("<th>BilboBaggins</th>", result.ToString());
        }

        #endregion

        #region Helper classes

        private class Model
        {
            public string Name { get; set; }
            public object AnotherProperty { get; set; }

            [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
            public DateTime FormattedDate { get; set; }
        }

        #endregion
    }
}
