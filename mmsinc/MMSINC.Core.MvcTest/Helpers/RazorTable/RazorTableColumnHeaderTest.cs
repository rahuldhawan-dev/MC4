using System.Globalization;
using System.Collections.Generic;
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
    public class RazorTableColumnHeaderTest
    {
        #region Fields

        private FakeMvcApplicationTester _tester;
        private FakeMvcHttpHandler _pipeline;
        private HtmlHelper<Model> _htmlHelper;
        private RazorTableColumnHeader<Model> _target;
        private Mock<IRazorTableColumn<Model>> _column;

        #endregion

        #region Init

        [TestInitialize]
        public void InitializeTest()
        {
            _tester = new FakeMvcApplicationTester(new Container());
            _tester.ControllerFactory.RegisterController("Home", new FakeCrudController());
            _pipeline = _tester.CreateRequestHandler();
            _htmlHelper = _pipeline.CreateHtmlHelper<Model>();
            _target = new RazorTableColumnHeader<Model>();
            _column = new Mock<IRazorTableColumn<Model>>();
        }

        [TestCleanup]
        public void CleanupTest()
        {
            _tester.Dispose();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestRendersTextWithHtmlEscaped()
        {
            _target.Text = "R & D";
            var result = _target.Render(_htmlHelper, _column.Object, null).ToString();
            Assert.AreEqual("<th>R &amp; D</th>", result);
        }

        [TestMethod]
        public void TestRendersEmptyTagWhenHtmlIsNullOrEmptyOrWhiteSpace()
        {
            var nullishStrings = new[] {string.Empty, null, "  "};
            foreach (var s in nullishStrings)
            {
                _target.Text = s;
                var result = _target.Render(_htmlHelper, _column.Object, null).ToString();
                Assert.AreEqual("<th></th>", result);
            }
        }

        [TestMethod]
        public void TestRendersSortingLinkWithCurrentRouteDataMergedWithPagingParameters()
        {
            var sortedSet = new Mock<ISearchSet<Model>>();
            sortedSet.Setup(x => x.Results).Returns(new List<Model>());
            _column.Setup(x => x.IsSortable).Returns(true);
            _column.Setup(x => x.SortBy).Returns("SortMe");
            _pipeline.RouteData.Values["SomeExistingRouteData"] = "42";

            const string urlFormat =
                @"/?SomeExistingRouteData=42&amp;PageNumber={0}&amp;SortBy={1}&amp;SortAscending={2}&amp;PageSize={3}";

            // Expect this to be the opposite.
            var asc = !sortedSet.Object.SortAscending;
            var expectedUrl = string.Format(urlFormat, sortedSet.Object.PageNumber, "SortMe", asc,
                sortedSet.Object.PageSize);
            var result = _target.Render(_htmlHelper, _column.Object, sortedSet.Object).ToString();

            Assert.IsTrue(result.Contains(expectedUrl), "Expected to find '{0}' inside all this: '{1}'", expectedUrl,
                result);
        }

        [TestMethod]
        public void TestRendersSortingLinkWithCurrentModelStateSerializedIntoQueryString()
        {
            var sortedSet = new Mock<ISearchSet<Model>>();
            sortedSet.Setup(x => x.Results).Returns(new List<Model>());
            _column.Setup(x => x.IsSortable).Returns(true);
            _column.Setup(x => x.SortBy).Returns("SortMe");
            _htmlHelper.ViewData.ModelState.Add("SomeModel.Yippy",
                new ModelState
                    {Value = new ValueProviderResult("Some Value", "Some Value", CultureInfo.CurrentCulture)});

            const string urlFormat =
                @"/?SomeModel.Yippy=Some%20Value&amp;PageNumber={0}&amp;SortBy={1}&amp;SortAscending={2}&amp;PageSize={3}";

            // Expect this to be the opposite.
            var asc = !sortedSet.Object.SortAscending;
            var expectedUrl = string.Format(urlFormat, sortedSet.Object.PageNumber, "SortMe", asc,
                sortedSet.Object.PageSize);
            var result = _target.Render(_htmlHelper, _column.Object, sortedSet.Object).ToString();

            Assert.IsTrue(result.Contains(expectedUrl), "Expected to find '{0}' inside all this: '{1}'", expectedUrl,
                result);
        }

        #endregion

        #region Helper class

        public class Model
        {
            public string Name { get; set; }
        }

        #endregion
    }
}
