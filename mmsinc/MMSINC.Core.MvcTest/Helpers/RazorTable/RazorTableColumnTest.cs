using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Helpers;
using MMSINC.Testing;
using StructureMap;

namespace MMSINC.Core.MvcTest.Helpers.RazorTable
{
    [TestClass]
    public class RazorTableColumnTest
    {
        #region Fields

        private FakeMvcHttpHandler _request;
        private TestColumn _target;
        private Model _model;
        private HtmlHelper<Model> _htmlHelper;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _request = new FakeMvcHttpHandler(new Container());
            _target = new TestColumn();
            _model = new Model {Name = "Dude"};
            _htmlHelper = _request.CreateHtmlHelper(_model);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestIsVisibleIsTrueByDefault()
        {
            _target = new TestColumn();
            Assert.IsTrue(_target.IsVisible);
        }

        [TestMethod]
        public void TestRenderCellReturnsEmptyTdTagBuilder()
        {
            var result = _target.RenderCell(_htmlHelper).ToString();
            Assert.AreEqual("<td></td>", result);
        }

        [TestMethod]
        public void TestRenderCellInvokesCellBuilderIfNotNull()
        {
            _target.CellBuilder = null;
            var result = _target.RenderCell(_htmlHelper).ToString();
            Assert.AreEqual("<td></td>", result);

            _target.CellBuilder = (x, tag) => { tag.InnerHtml = "I am cell built!"; };

            result = _target.RenderCell(_htmlHelper).ToString();
            Assert.AreEqual("<td>I am cell built!</td>", result);
        }

        [TestMethod]
        public void TestRenderCellInvokesCellBuilderAfterItRendersTheCellValue()
        {
            // This is testing that CellBuilder can overwrite whatever the Text/InnerHtml
            // was set to by the RenderCell override inheritors use. TestRenderText is being
            // used by that since the base RazorColumn class doesn't do any value rendering.

            _target.TestRenderText = "Default text";
            _target.CellBuilder = null;
            var result = _target.RenderCell(_htmlHelper).ToString();
            Assert.AreEqual("<td>Default text</td>", result);

            _target.CellBuilder = (x, tag) => { tag.InnerHtml = "I am cell built!"; };

            result = _target.RenderCell(_htmlHelper).ToString();
            Assert.AreEqual("<td>I am cell built!</td>", result);
        }

        [TestMethod]
        public void TestRenderHeaderUsesTheColumnHeadersRenderMethod()
        {
            _target.Header = new RazorTableColumnHeader<Model> {
                Text = "Some Text"
            };
            var result = _target.RenderColumnHeader(_htmlHelper).ToString();
            Assert.AreEqual("<th>Some Text</th>", result);
        }

        [TestMethod]
        public void TestRenderHeaderPassesSelfToHeadersRenderMethod()
        {
            var header = new TestColumnHeader();
            _target.Header = header;
            _target.SortBy = "Something";
            _target.RenderColumnHeader(_htmlHelper);
            Assert.AreSame(_target, header.ColumnPassedToRender);
        }

        #endregion

        #region Helper classes

        private class Model
        {
            public string Name { get; set; }
            public object AnotherProperty { get; set; }
        }

        private class TestColumn : RazorTableColumn<Model>
        {
            public string TestRenderText { get; set; }

            protected override void RenderCell(HtmlHelper<Model> helper, TagBuilder tagBuilder)
            {
                if (TestRenderText != null)
                {
                    tagBuilder.InnerHtml = TestRenderText;
                }

                // else no op
            }
        }

        private class TestColumnHeader : IRazorTableColumnHeader<Model>
        {
            public IRazorTableColumn<Model> ColumnPassedToRender { get; private set; }

            public TagBuilder Render(HtmlHelper<Model> helper, IRazorTableColumn<Model> column, ISearchSet sortedSet)
            {
                ColumnPassedToRender = column;
                return null;
            }
        }

        #endregion
    }
}
