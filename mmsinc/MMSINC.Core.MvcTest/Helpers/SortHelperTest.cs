using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Helpers;
using Moq;

namespace MMSINC.Core.MvcTest.Helpers
{
    [TestClass]
    public class SortHelperTest
    {
        #region Private Members

        private HtmlHelper _target;
        private Func<int, string, bool, int, string> _pageUrlFn;
        private Mock<ISearchSet> _resultsSet;

        #endregion

        #region Setup/TearDown

        [TestInitialize]
        public void TestInitialize()
        {
            _resultsSet = new Mock<ISearchSet>();
            _resultsSet.SetupGet(x => x.PageCount).Returns(1);
            _resultsSet.SetupGet(x => x.PageNumber).Returns(1);
            _resultsSet.SetupGet(x => x.PageSize).Returns(10);
            _resultsSet.SetupGet(x => x.SortAscending).Returns(true);
            _resultsSet.SetupGet(x => x.SortBy).Returns("Foo");

            _target = new HtmlHelper(new ViewContext(),
                new Mock<IViewDataContainer>().Object);
        }

        #endregion

        [TestMethod]
        public void TestSortByLinkCreatesCorrectSortLinks()
        {
            string expected =
                "<a href=\"page_1__True_10\"></a>";

            _pageUrlFn = (i, j, k, l) => String.Format("page_{0}_{1}_{2}_{3}", i, j, k, l);

            Assert.AreEqual(expected, _target.SortByLink(_resultsSet.Object, "", "", _pageUrlFn).ToString());

            expected =
                "<a href=\"page_1__True_10\">headerText</a>";

            Assert.AreEqual(
                expected,
                _target.SortByLink(_resultsSet.Object, "", "headerText", _pageUrlFn).ToString(),
                "Failed to place header text in the A tag");

            expected =
                "<a href=\"page_1_sortBy_True_10\">headerText</a>";

            Assert.AreEqual(
                expected,
                _target.SortByLink(_resultsSet.Object, "sortBy", "headerText", _pageUrlFn).ToString(),
                "Failed to place sortBy in the link");

            expected =
                "<a href=\"page_1_Foo_False_10\">headerText</a><span>▾</span>";

            Assert.AreEqual(
                expected,
                _target.SortByLink(_resultsSet.Object, "Foo", "headerText", _pageUrlFn).ToString(),
                "Failed to place flip the sort direction for the link");

            expected =
                "<a href=\"page_1_HeaderField_True_10\">Header Field</a>";

            Assert.AreEqual(
                expected,
                _target.SortByLink(_resultsSet.Object, "HeaderField", _pageUrlFn).ToString(),
                "Failed to use the override to generate title case for link text");
            expected =
                "<a href=\"page_1_Foo_True_10\">headerText</a><span>▴</span>";
            _resultsSet.SetupGet(x => x.SortAscending).Returns(false);

            Assert.AreEqual(
                expected,
                _target.SortByLink(_resultsSet.Object, "Foo", "headerText", _pageUrlFn).ToString(),
                "Failed to place flip the sort direction for the link");
        }
    }
}
