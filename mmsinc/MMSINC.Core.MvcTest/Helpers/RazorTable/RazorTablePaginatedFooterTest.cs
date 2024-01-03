using System;
using System.Web.Mvc;
using MMSINC.Data;
using MMSINC.Helpers;
using MMSINC.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StructureMap;

namespace MMSINC.Core.MvcTest.Helpers.RazorTable
{
    [TestClass]
    public class RazorTablePaginatedFooterTest
    {
        #region Fields

        private FakeMvcApplicationTester _tester;
        private RazorTablePaginatedFooter _target;
        private HtmlHelper _htmlHelper;
        private Mock<ISearchSet> _mockModel;

        #endregion

        #region Init

        [TestInitialize]
        public void InitializeTest()
        {
            _tester = new FakeMvcApplicationTester(new Container());
            _htmlHelper = _tester.CreateRequestHandler().CreateHtmlHelper<object>();
            _mockModel = new Mock<ISearchSet>();
            _target = new RazorTablePaginatedFooter(_mockModel.Object);
        }

        [TestCleanup]
        public void CleanupTest()
        {
            _tester.Dispose();
        }

        #endregion

        #region Private Methods

        private string RenderTarget()
        {
            var result = _target.Render(_htmlHelper, 42).ToString();
            // TC has different NewLines or something. I dunno.
            result = result.Replace(Environment.NewLine, string.Empty);
            return result;
        }

        private void AssertTargetRendersEmptyTFootTag()
        {
            var result = RenderTarget();
            Assert.AreEqual("<div class=\"table-footer\"></div>", result);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestRenderReturnsDivTag()
        {
            var result = _target.Render(_htmlHelper, 42);
            Assert.AreEqual("div", result.TagName);
        }

        [TestMethod]
        public void TestRendersEmptyTagWhenModelIsNull()
        {
            _target = new RazorTablePaginatedFooter(null);
            AssertTargetRendersEmptyTFootTag();
        }

        [TestMethod]
        public void TestRendersEmptyTagWhenThereIsOnlyOnePageInTheSetAndSetCountIsLessThan1()
        {
            _mockModel.Setup(x => x.PageCount).Returns(1);
            _mockModel.Setup(x => x.Count).Returns(0);
            AssertTargetRendersEmptyTFootTag();
            _mockModel.Setup(x => x.Count).Returns(1);
            AssertTargetRendersEmptyTFootTag();
        }

        [TestMethod]
        public void TestRendersPageLinksWhenPageCountIsGreaterThan1()
        {
            var expected = "<div class=\"table-footer\">" +
                           "<div class=\"page-links\">" +
                           "<a class=\"paginationLink\" href=\"/?PageNumber=1&amp;SortAscending=False&amp;PageSize=0\">1</a>" +
                           "<a class=\"paginationLink\" href=\"/?PageNumber=2&amp;SortAscending=False&amp;PageSize=0\">2</a>" +
                           "<a class=\"paginationLink\" href=\"/?PageNumber=1&amp;SortAscending=False&amp;PageSize=0\">&gt;&gt;</a>" +
                           "</div></div>";
            expected = expected.Replace(Environment.NewLine, string.Empty);
            _mockModel.Setup(x => x.PageCount).Returns(2);
            var result = RenderTarget();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestRendersResultsPerPageLinksWhenModelCountIsGreaterThan1()
        {
            var expected = "<div class=\"table-footer\">" +
                           "<div class=\"page-links-results-per-page\">Results Per Page: ( " + // See that white space at the end there? IMPORTANT
                           "<a class=\"paginationLink\" href=\"/?PageNumber=1&amp;SortAscending=False&amp;PageSize=1\">1</a>" +
                           "<a class=\"paginationLink\" href=\"/?PageNumber=1&amp;SortAscending=False&amp;PageSize=2\">All</a> )" +
                           "</div></div>";
            expected = expected.Replace(Environment.NewLine, string.Empty);

            _mockModel.Setup(x => x.PageCount).Returns(1);
            _mockModel.Setup(x => x.Count).Returns(2);
            var result = RenderTarget();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestDoesNotRenderResultsPerPageLinksWhenModelCountIsGreaterThan1AndShowPageSizeOptionsIsFalse()
        {
            var expected = "<div class=\"table-footer\"></div>";
            expected = expected.Replace(Environment.NewLine, string.Empty);

            _mockModel.Setup(x => x.PageCount).Returns(1);
            _mockModel.Setup(x => x.Count).Returns(2);
            _target.ShowPageSizeOptions = false;
            var result = RenderTarget();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void
            TestRendersBothPageLinksAndResultsPerPageSectionsWhenPageCountIsGreaterThan1AndModelCountIsGreaterThan1()
        {
            var expected = "<div class=\"table-footer\">" +
                           "<div class=\"page-links\">" +
                           "<a class=\"paginationLink\" href=\"/?PageNumber=1&amp;SortAscending=False&amp;PageSize=0\">1</a>" +
                           "<a class=\"paginationLink\" href=\"/?PageNumber=2&amp;SortAscending=False&amp;PageSize=0\">2</a>" +
                           "<a class=\"paginationLink\" href=\"/?PageNumber=1&amp;SortAscending=False&amp;PageSize=0\">&gt;&gt;</a>" +
                           "</div><div class=\"page-links-results-per-page\">Results Per Page: ( " + // See that white space at the end there? IMPORTANT
                           "<a class=\"paginationLink\" href=\"/?PageNumber=1&amp;SortAscending=False&amp;PageSize=1\">1</a>" +
                           "<a class=\"paginationLink\" href=\"/?PageNumber=1&amp;SortAscending=False&amp;PageSize=2\">All</a> )" +
                           "</div></div>";
            expected = expected.Replace(Environment.NewLine, string.Empty);

            _mockModel.Setup(x => x.PageCount).Returns(2);
            _mockModel.Setup(x => x.Count).Returns(2);
            var result = RenderTarget();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestDoesNotRenderPageLinksWhenThereIsOnlyOnePageInTheSet()
        {
            var expected = "<div class=\"table-footer\">" +
                           "<div class=\"page-links-results-per-page\">Results Per Page: ( " +
                           "<a class=\"paginationLink\" href=\"/?PageNumber=1&amp;SortAscending=False&amp;PageSize=1\">1</a>" +
                           "<a class=\"paginationLink\" href=\"/?PageNumber=1&amp;SortAscending=False&amp;PageSize=2\">All</a> )" +
                           "</div></div>";
            expected = expected.Replace(Environment.NewLine, string.Empty);

            _mockModel.Setup(x => x.PageCount).Returns(1);
            _mockModel.Setup(x => x.Count).Returns(2);
            var result = RenderTarget();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestDoesNotRenderResultsPerPageWhenTheModelsCountIsLessThanOrEqualToOne()
        {
            var expected =
                "<div class=\"table-footer\">" +
                "<div class=\"page-links\">" +
                "<a class=\"paginationLink\" href=\"/?PageNumber=1&amp;SortAscending=False&amp;PageSize=0\">1</a>" +
                "<a class=\"paginationLink\" href=\"/?PageNumber=2&amp;SortAscending=False&amp;PageSize=0\">2</a>" +
                "<a class=\"paginationLink\" href=\"/?PageNumber=1&amp;SortAscending=False&amp;PageSize=0\">&gt;&gt;</a>" +
                "</div></div>";
            expected = expected.Replace(Environment.NewLine, string.Empty);

            _mockModel.Setup(x => x.PageCount).Returns(2);

            foreach (var count in new[] {0, 1})
            {
                _mockModel.Setup(x => x.Count).Returns(count);
                var result = RenderTarget();
                Assert.AreEqual(expected, result);
            }
        }

        #endregion
    }
}
