using System;
using System.Linq;
using System.Web.Mvc;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Data;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MMSINC.Helpers;
using MMSINC.Utilities.StructureMap;
using StructureMap;

namespace MMSINC.Core.MvcTest.Helpers
{
    [TestClass]
    public class PaginationHelperTest
    {
        #region Private Members

        private HtmlHelper _target;
        private Func<int, string, bool, int, string> _pageUrlFn;
        private IContainer _container;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _target = new HtmlHelper(new ViewContext(),
                new Mock<IViewDataContainer>().Object);
            _container = new Container();
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(_container));
        }

        #endregion

        #region Tests

        #region Configuration

        [TestMethod]
        public void TestConfigurationReturnsConfigurationRegisteredWithObjectFactoryIfOneIsRegistered()
        {
            var expected = new PaginationHelperConfiguration();
            _container.Inject(expected);
            Assert.AreSame(expected, PaginationHelper.Configuration);

            _container.EjectAllInstancesOf<PaginationHelperConfiguration>();
            Assert.IsNotNull(PaginationHelper.Configuration);
            Assert.AreNotSame(expected, PaginationHelper.Configuration);
        }

        #endregion

        #region PaginationLinks

        [TestMethod]
        public void TestPaginationLinksWithAjaxOptionsAppendsThoseAjaxOptionsToLinks()
        {
            Assert.Inconclusive("TODO");
        }

        [TestMethod]
        public void TestPaginationLinksReturnsEmptyStringIfSetContainsJustOnePage()
        {
            var set = new Mock<ISearchSet>();
            set.SetupGet(x => x.PageCount).Returns(1);

            Assert.AreEqual(String.Empty,
                _target.PaginationLinks(set.Object, _pageUrlFn).ToString());
        }

        [TestMethod]
        public void TestPaginationLinksReturnsPageOneAndPreviousPageLinksWhenTwoPagesAndOnPageTwo()
        {
            var expected =
                "<a class=\"paginationLink\" href=\"page_1\">&lt;&lt;</a><a class=\"paginationLink\" href=\"page_1\">1</a><span>2</span>";
            var set = new Mock<ISearchSet>();
            _pageUrlFn = (i, j, k, l) => String.Format("page_{0}", i);
            set.SetupGet(x => x.PageCount).Returns(2);
            set.SetupGet(x => x.PageNumber).Returns(2);

            var result = _target.PaginationLinks(set.Object, _pageUrlFn).ToString();

            Assert.AreEqual(expected,
                result.Replace(Environment.NewLine, String.Empty));
        }

        [TestMethod]
        public void TestPaginationLinksReturnsPageOneAndPreviousPageWithSortByLinksWhenTwoPagesAndOnPageTwo()
        {
            var expected =
                "<a class=\"paginationLink\" href=\"page_1_sortByFoo\">&lt;&lt;</a><a class=\"paginationLink\" href=\"page_1_sortByFoo\">1</a><span>2</span>";
            var set = new Mock<ISearchSet>();
            _pageUrlFn = (i, j, k, l) => String.Format("page_{0}_{1}", i, j, l);
            set.SetupGet(x => x.PageCount).Returns(2);
            set.SetupGet(x => x.PageNumber).Returns(2);
            set.SetupGet(x => x.SortBy).Returns("sortByFoo");

            var result = _target.PaginationLinks(set.Object, _pageUrlFn).ToString();

            Assert.AreEqual(expected,
                result.Replace(Environment.NewLine, String.Empty));
        }

        [TestMethod]
        public void
            TestPaginationLinksReturnsPageOneAndPreviousPageWithSortBySortDirectionLinksWhenTwoPagesAndOnPageTwo()
        {
            var expected =
                "<a class=\"paginationLink\" href=\"page_1_sortByFoo_True_10\">&lt;&lt;</a><a class=\"paginationLink\" href=\"page_1_sortByFoo_True_10\">1</a><span>2</span>";
            var set = new Mock<ISearchSet>();
            _pageUrlFn = (i, j, k, l) => String.Format("page_{0}_{1}_{2}_{3}", i, j, k, l);
            set.SetupGet(x => x.PageCount).Returns(2);
            set.SetupGet(x => x.PageNumber).Returns(2);
            set.SetupGet(x => x.SortBy).Returns("sortByFoo");
            set.SetupGet(x => x.PageSize).Returns(10);
            set.SetupGet(x => x.SortAscending).Returns(true);

            var result = _target.PaginationLinks(set.Object, _pageUrlFn).ToString();

            Assert.AreEqual(expected,
                result.Replace(Environment.NewLine, String.Empty));
        }

        [TestMethod]
        public void TestPaginationLinksReturnsPageTwoAndNextPageLinksWhenTwoPagesAndOnPageOne()
        {
            var expected =
                "<span>1</span><a class=\"paginationLink\" href=\"page_2\">2</a><a class=\"paginationLink\" href=\"page_2\">&gt;&gt;</a>";
            var set = new Mock<ISearchSet>();
            _pageUrlFn = (i, j, k, l) => String.Format("page_{0}", i);
            set.SetupGet(x => x.PageCount).Returns(2);
            set.SetupGet(x => x.PageNumber).Returns(1);

            var result = _target.PaginationLinks(set.Object, _pageUrlFn).ToString();

            Assert.AreEqual(expected,
                result.Replace(Environment.NewLine, String.Empty));
        }

        [TestMethod]
        public void TestPaginationLinksReturnsPageTwoAndNextPageLinksWithSortByWhenTwoPagesAndOnPageOne()
        {
            var expected =
                "<span>1</span><a class=\"paginationLink\" href=\"page_2_sortByBar_10\">2</a><a class=\"paginationLink\" href=\"page_2_sortByBar_10\">&gt;&gt;</a>";
            var set = new Mock<ISearchSet>();
            _pageUrlFn = (i, j, k, l) => String.Format("page_{0}_{1}_{2}", i, j, l);
            set.SetupGet(x => x.PageCount).Returns(2);
            set.SetupGet(x => x.PageNumber).Returns(1);
            set.SetupGet(x => x.PageSize).Returns(10);
            set.SetupGet(x => x.SortBy).Returns("sortByBar");

            var result = _target.PaginationLinks(set.Object, _pageUrlFn).ToString();

            Assert.AreEqual(expected,
                result.Replace(Environment.NewLine, String.Empty));
        }

        [TestMethod]
        public void TestPaginationLinksReturnsPageTwoAndNextPageLinksWithSortBySortDirectionWhenTwoPagesAndOnPageOne()
        {
            var expected =
                "<span>1</span><a class=\"paginationLink\" href=\"page_2_sortByBar_False_10\">2</a><a class=\"paginationLink\" href=\"page_2_sortByBar_False_10\">&gt;&gt;</a>";
            var set = new Mock<ISearchSet>();
            _pageUrlFn = (i, j, k, l) => String.Format("page_{0}_{1}_{2}_{3}", i, j, k, l);
            set.SetupGet(x => x.PageCount).Returns(2);
            set.SetupGet(x => x.PageNumber).Returns(1);
            set.SetupGet(x => x.PageSize).Returns(10);
            set.SetupGet(x => x.SortBy).Returns("sortByBar");
            set.SetupGet(x => x.SortAscending).Returns(false);

            var result = _target.PaginationLinks(set.Object, _pageUrlFn).ToString();

            Assert.AreEqual(expected,
                result.Replace(Environment.NewLine, String.Empty));
        }

        [TestMethod]
        public void TestPaginationLinksReturnsSplitUpResultsForLargeNumbersOfPagesBasedOnConfiguration()
        {
            var config = new PaginationHelperConfiguration {
                BookEndLinkCount = 2,
                MiddleLinkCount = 2
            };

            // Should see two links, a class=""paginationLink"" separator, two more links, a class=""paginationLink"" separator, two more links, and a class=""paginationLink"" next link
            var expected = "<span>1</span>" +
                           "<a class=\"paginationLink\" href=\"page_2_sortByBar_False_10\">2</a>" +
                           "<span>...</span>" +
                           "<a class=\"paginationLink\" href=\"page_9_sortByBar_False_10\">9</a>" +
                           "<a class=\"paginationLink\" href=\"page_10_sortByBar_False_10\">10</a>" +
                           "<span>...</span>" +
                           "<a class=\"paginationLink\" href=\"page_19_sortByBar_False_10\">19</a>" +
                           "<a class=\"paginationLink\" href=\"page_20_sortByBar_False_10\">20</a>" +
                           "<a class=\"paginationLink\" href=\"page_2_sortByBar_False_10\">&gt;&gt;</a>";

            var set = new Mock<ISearchSet>();
            _pageUrlFn = (i, j, k, l) => String.Format("page_{0}_{1}_{2}_{3}", i, j, k, l);
            set.SetupGet(x => x.PageCount).Returns(20);
            set.SetupGet(x => x.PageNumber).Returns(1);
            set.SetupGet(x => x.PageSize).Returns(10);
            set.SetupGet(x => x.SortBy).Returns("sortByBar");
            set.SetupGet(x => x.SortAscending).Returns(false);

            try
            {
                _container.Inject(config);
                var result = _target.PaginationLinks(set.Object, _pageUrlFn).ToString()
                                    .Replace(Environment.NewLine, String.Empty);
                Assert.AreEqual(expected, result);
            }
            finally
            {
                _container.EjectAllInstancesOf<PaginationHelperConfiguration>();
            }
        }

        #endregion

        #region PaginationFooter

        [TestMethod]
        public void TestPaginationFooterReturnsWithLinksForPageSizeWhenCountLessThan5()
        {
            var expected = @"Results Per Page: ( 
<a class=""paginationLink"" href=""page_1_sortByBar_False_1"">1</a>
<a class=""paginationLink"" href=""page_1_sortByBar_False_4"">All</a>
 )
";
            var set = new Mock<ISearchSet>();
            _pageUrlFn = (i, j, k, l) => String.Format("page_{0}_{1}_{2}_{3}", i, j, k, l);
            set.SetupGet(x => x.Count).Returns(4);
            set.SetupGet(x => x.PageNumber).Returns(1);
            set.SetupGet(x => x.PageSize).Returns(10);
            set.SetupGet(x => x.SortBy).Returns("sortByBar");

            var result = _target.PaginationFooter(set.Object, _pageUrlFn).ToString();

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestPaginationFooterReturnsWithLinksForPageSizeWhenCountLessThan10GreaterThan5()
        {
            var expected = @"Results Per Page: ( 
<a class=""paginationLink"" href=""page_1_sortByBar_False_1"">1</a>
<a class=""paginationLink"" href=""page_1_sortByBar_False_5"">5</a>
<a class=""paginationLink"" href=""page_1_sortByBar_False_9"">All</a>
 )
";
            var set = new Mock<ISearchSet>();
            _pageUrlFn = (i, j, k, l) => String.Format("page_{0}_{1}_{2}_{3}", i, j, k, l);
            set.SetupGet(x => x.Count).Returns(9);
            set.SetupGet(x => x.PageNumber).Returns(1);
            set.SetupGet(x => x.PageSize).Returns(10);
            set.SetupGet(x => x.SortBy).Returns("sortByBar");

            var result = _target.PaginationFooter(set.Object, _pageUrlFn).ToString();

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestPaginationFooterReturnsWithLinksForPageSizeWhenCountLessThan25GreaterThan10()
        {
            var expected = @"Results Per Page: ( 
<a class=""paginationLink"" href=""page_1_sortByBar_False_1"">1</a>
<a class=""paginationLink"" href=""page_1_sortByBar_False_5"">5</a>
<a class=""paginationLink"" href=""page_1_sortByBar_False_10"">10</a>
<a class=""paginationLink"" href=""page_1_sortByBar_False_24"">All</a>
 )
";
            var set = new Mock<ISearchSet>();
            _pageUrlFn = (i, j, k, l) => String.Format("page_{0}_{1}_{2}_{3}", i, j, k, l);
            set.SetupGet(x => x.Count).Returns(24);
            set.SetupGet(x => x.PageNumber).Returns(1);
            set.SetupGet(x => x.PageSize).Returns(10);
            set.SetupGet(x => x.SortBy).Returns("sortByBar");

            var result = _target.PaginationFooter(set.Object, _pageUrlFn).ToString();

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestPaginationFooterReturnsAllWhenCountGreaterThan25AndLessThan500()
        {
            var expected = @"Results Per Page: ( 
<a class=""paginationLink"" href=""page_1_sortByBar_False_1"">1</a>
<a class=""paginationLink"" href=""page_1_sortByBar_False_5"">5</a>
<a class=""paginationLink"" href=""page_1_sortByBar_False_10"">10</a>
<a class=""paginationLink"" href=""page_1_sortByBar_False_25"">25</a>
<a class=""paginationLink"" href=""page_1_sortByBar_False_322"">All</a>
 )
";
            var set = new Mock<ISearchSet>();
            _pageUrlFn = (i, j, k, l) => String.Format("page_{0}_{1}_{2}_{3}", i, j, k, l);
            set.SetupGet(x => x.Count).Returns(322);
            set.SetupGet(x => x.PageNumber).Returns(1);
            set.SetupGet(x => x.PageSize).Returns(10);
            set.SetupGet(x => x.SortBy).Returns("sortByBar");

            var result = _target.PaginationFooter(set.Object, _pageUrlFn).ToString();

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestPaginationFooterReturnsAllWhenCountGreaterThan500()
        {
            var expected = @"Results Per Page: ( 
<a class=""paginationLink"" href=""page_1_sortByBar_False_1"">1</a>
<a class=""paginationLink"" href=""page_1_sortByBar_False_5"">5</a>
<a class=""paginationLink"" href=""page_1_sortByBar_False_10"">10</a>
<a class=""paginationLink"" href=""page_1_sortByBar_False_25"">25</a>
<a class=""paginationLink"" href=""page_1_sortByBar_False_500"">500</a>
 )
";
            var set = new Mock<ISearchSet>();
            _pageUrlFn = (i, j, k, l) => String.Format("page_{0}_{1}_{2}_{3}", i, j, k, l);
            set.SetupGet(x => x.Count).Returns(501);
            set.SetupGet(x => x.PageNumber).Returns(1);
            set.SetupGet(x => x.PageSize).Returns(10);
            set.SetupGet(x => x.SortBy).Returns("sortByBar");

            var result = _target.PaginationFooter(set.Object, _pageUrlFn).ToString();

            Assert.AreEqual(expected, result);
        }

        #endregion

        #region LinkablePageNumbers

        [TestMethod]
        public void TestLinkablePageNumbersThrowsExceptionIfActualPageCountIsLessThanZero()
        {
            var set = new Mock<ISearchSet>();
            set.Setup(x => x.PageNumber).Returns(1);

            foreach (var i in IEnumerableExtensions.Range(-100, 0)) // will test values from -100 up to 0. 
            {
                set.Setup(x => x.PageCount).Returns(i);
                MyAssert.Throws<InvalidOperationException>(() =>
                    PaginationHelper.GetLinkablePageNumbers(1, 1, set.Object));
            }
        }

        [TestMethod]
        public void
            TestGetPageNumbersWorthyOfLinksReturnsAllPageNumbersAsValidWhenTheTotalNumberOfPagesIsLessThanTheNumberOfLinksThatWouldBeDisplayedInJustTheBookEnds()
        {
            var expectedBookEndCount = 5;
            var expectedMiddleCount = 0;
            var maxPageCount = (expectedBookEndCount * 2 + expectedMiddleCount);
            Assert.AreEqual(10, maxPageCount, "I can't do math");

            var set = new Mock<ISearchSet>();
            set.Setup(x => x.PageNumber).Returns(1);

            foreach (var i in IEnumerableExtensions.Range(1, maxPageCount))
            {
                set.Setup(x => x.PageCount).Returns(i);
                Assert.IsTrue(PaginationHelper
                             .GetLinkablePageNumbers(expectedBookEndCount, expectedMiddleCount, set.Object)
                             .AllPagesAreValid);
            }

            set.Setup(x => x.PageCount).Returns(maxPageCount + 1);
            var result = PaginationHelper.GetLinkablePageNumbers(expectedBookEndCount, expectedMiddleCount, set.Object);
            Assert.IsFalse(result.AllPagesAreValid);
        }

        private void AssertValidRange(int bookEndCount, int middleCount, int maxPage, int currentPageNumber,
            int beginStart, int beginEnd, int midStart, int midEnd, int endStart, int endEnd)
        {
            var set = new Mock<ISearchSet>();
            set.Setup(x => x.PageNumber).Returns(currentPageNumber);
            set.Setup(x => x.PageCount).Returns(maxPage);
            var result = PaginationHelper.GetLinkablePageNumbers(bookEndCount, middleCount, set.Object);

            Assert.IsFalse(result.AllPagesAreValid,
                "AllPagesAreValid must be false for this test to pass. Either test params are bad or the class is calculating wrong.");

            // Test that these are all right and stuff.
            Assert.AreEqual(bookEndCount, result.Beginning.Count(), "Beginning count. First value {0}, Last value: {1}",
                result.Beginning.First(), result.Beginning.Last());
            Assert.AreEqual(bookEndCount, result.End.Count(), "Ending count. First value {0}, Last value: {1}",
                result.End.First(), result.End.Last());
            Assert.AreEqual(middleCount, result.Middle.Count(), "Middle count. First value {0}, Last value: {1}",
                result.Middle.First(), result.Middle.Last());

            Assert.AreEqual(beginStart, result.Beginning.First(), "First link for beginning group");
            Assert.AreEqual(beginEnd, result.Beginning.Last(), "Last link for beginning group");

            Assert.AreEqual(midStart, result.Middle.First(), "First link for middle group");
            Assert.AreEqual(midEnd, result.Middle.Last(), "Last link for middle group");

            Assert.AreEqual(endStart, result.End.First(), "First link for end group");
            Assert.AreEqual(endEnd, result.End.Last(), "Last link for end group");
        }

        [TestMethod]
        public void
            TestGetLinkablePageNumbersReturnsEmptyEnumerableForMiddleRangeWhenMiddleCountIsLessThanOrEqualToZero()
        {
            var set = new Mock<ISearchSet>();
            set.Setup(x => x.PageNumber).Returns(1);
            set.Setup(x => x.PageCount).Returns(234);

            foreach (var lessThanOrEqualToZero in IEnumerableExtensions.Range(-100, 0))
            {
                Assert.IsFalse(PaginationHelper.GetLinkablePageNumbers(5, lessThanOrEqualToZero, set.Object).Middle
                                               .Any());
            }
        }

        [TestMethod]
        public void TestGetLinkablePageNumbersReturnsExpectedMiddleRanges()
        {
            // As the middle range count increases, differences between first/last page number
            // start with decreasing the first page number, then increasing the last page number.
            AssertValidRange(5, 1, 20, 1, 1, 5, 10, 10, 16, 20);
            AssertValidRange(5, 2, 20, 1, 1, 5, 9, 10, 16, 20);
            AssertValidRange(5, 3, 20, 1, 1, 5, 9, 11, 16, 20);
            AssertValidRange(5, 4, 20, 1, 1, 5, 8, 11, 16, 20);
            AssertValidRange(5, 5, 20, 1, 1, 5, 8, 12, 16, 20);
            AssertValidRange(5, 6, 20, 1, 1, 5, 7, 12, 16, 20);
            AssertValidRange(5, 7, 20, 1, 1, 5, 7, 13, 16, 20);
        }

        [TestMethod]
        public void
            TestGetLinkablePageNumbersReturnsExpectedMiddleRangesWhenTheCurrentPageIsActuallyInTheMiddleSomewhere()
        {
            AssertValidRange(5, 1, 100, 30, 1, 5, 30, 30, 96, 100);
            AssertValidRange(5, 1, 100, 50, 1, 5, 50, 50, 96, 100);

            // Current page is in beginning group, middle links should be actual middle
            AssertValidRange(5, 1, 100, 5, 1, 5, 50, 50, 96, 100);
            // Current page is in end group, middle links should be actual middle
            AssertValidRange(5, 1, 100, 96, 1, 5, 50, 50, 96, 100);

            // Current page or one of the surrounding middle links collides with the
            // beginning group of links, collisions should shift middle link collection forward.
            AssertValidRange(5, 2, 100, 5, 1, 5, 6, 7, 96, 100);
            AssertValidRange(5, 2, 100, 6, 1, 5, 6, 7, 96, 100);
            AssertValidRange(5, 2, 100, 7, 1, 5, 6, 7, 96, 100);
            AssertValidRange(5, 2, 100, 8, 1, 5, 7, 8, 96, 100);
            AssertValidRange(5, 5, 477, 17, 1, 5, 15, 19, 473, 477);

            // Current page or one of the surrounding middle links collides with the
            // end group of links, collisions should shift middle link collection backward.
            AssertValidRange(5, 2, 100, 95, 1, 5, 94, 95, 96, 100);
        }

        [TestMethod]
        public void TestGetLinkablePageNumberReturnsExpectedBeginningAndEndingRanges()
        {
            AssertValidRange(1, 1, 20, 1, 1, 1, 10, 10, 20, 20);
            AssertValidRange(2, 1, 20, 1, 1, 2, 10, 10, 19, 20);
            AssertValidRange(3, 1, 20, 1, 1, 3, 10, 10, 18, 20);
            AssertValidRange(4, 1, 20, 1, 1, 4, 10, 10, 17, 20);
            AssertValidRange(5, 1, 20, 1, 1, 5, 10, 10, 16, 20);

            AssertValidRange(6, 1, 20, 1, 1, 6, 10, 10, 15, 20);
            AssertValidRange(7, 1, 20, 1, 1, 7, 10, 10, 14, 20);
            AssertValidRange(8, 1, 20, 1, 1, 8, 10, 10, 13, 20);
            AssertValidRange(9, 1, 20, 1, 1, 9, 10, 10, 12, 20);
        }

        #endregion

        #endregion
    }
}
