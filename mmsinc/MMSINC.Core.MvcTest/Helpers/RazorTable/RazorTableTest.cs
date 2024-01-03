using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.WebPages;
using MMSINC.Data;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Helpers;
using MMSINC.Testing;
using Moq;
using StructureMap;

namespace MMSINC.Core.MvcTest.Helpers.RazorTable
{
    [TestClass]
    public class RazorTableTest
    {
        #region Fields

        private FakeMvcApplicationTester _tester;
        private FakeMvcHttpHandler _pipeline;
        private RazorTable<Model> _target;
        private RazorTable<Model> _sortableTarget;
        private List<Model> _models;
        private TestPagingSearchSet _sortedModels;
        private HtmlHelper<IEnumerable<Model>> _htmlHelper;
        private Model _modelInstance;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _tester = new FakeMvcApplicationTester(new Container());
            _pipeline = _tester.CreateRequestHandler();
            _modelInstance = new Model {Name = "Some name"};
            var defaultTestModels = new[] {_modelInstance}.ToList();
            InitializeForModel(defaultTestModels);
        }

        [TestCleanup]
        public void CleanupTest()
        {
            _tester.Dispose();
        }

        private void InitializeForModel(List<Model> model)
        {
            _models = model;
            _sortedModels = new TestPagingSearchSet {
                Results = _models,
                Count = (_models != null ? _models.Count : 0),
                EnablePaging = true
            };
            var controller = _pipeline.CreateAndInitializeController<FakeController>();
            _htmlHelper = _pipeline.CreateHtmlHelper<IEnumerable<Model>>(controller, model);
            _target = new RazorTable<Model>(model, null) {HtmlHelper = _htmlHelper}
               .ColumnFor(x => x.Name);
            _sortableTarget = new RazorTable<Model>(_sortedModels, null) {HtmlHelper = _htmlHelper}
               .ColumnFor(x => x.Name);
        }

        #endregion

        #region Private Methods

        private Func<Model, HelperResult> CreateHelperResult(string returnValue)
        {
            Func<Model, HelperResult> result = (m) => {
                Action<TextWriter> actWriter = (tw) => { tw.Write(returnValue); };
                return new HelperResult(actWriter);
            };
            return result;
        }

        private void AssertRenders(RazorTable<Model> target, string header, string cellInnerHtml)
        {
            if (target.Model == null || target.Model.Count() != 1)
            {
                Assert.Fail("This test requires that a model with exactly one item be set.");
            }

            var expected =
                $"<div class=\"table-wrapper\"><table><thead><tr><th>{header}</th></tr></thead><tbody><tr><td>{cellInnerHtml}</td></tr></tbody></table></div>";
            var result = target.ToString();
            Assert.AreEqual(expected, result);
        }

        private void AssertRendersWithDataProperty(RazorTable<Model> target, string header, string dataProperty,
            string cellInnerHtml)
        {
            if (target.Model == null || target.Model.Count() != 1)
            {
                Assert.Fail("This test requires that a model with exactly one item be set.");
            }

            var expected =
                $"<div class=\"table-wrapper\"><table><thead><tr><th data-property=\"{dataProperty}\">{header}</th></tr></thead><tbody><tr><td>{cellInnerHtml}</td></tr></tbody></table></div>";
            var result = target.ToString();
            Assert.AreEqual(expected, result);
        }

        private void AssertRendersSorted(RazorTable<Model> target, ISearchSet<Model> sortedSet, string sortBy,
            string header, string cellInnerHtml)
        {
            if (target.Model == null || target.Model.Count() != 1)
            {
                Assert.Fail("This test requires that a model with exactly one item be set.");
            }

            // Expect this to be the opposite.
            var asc = !sortedSet.SortAscending;
            var expectedUrl =
                $"/?PageNumber={sortedSet.PageNumber}&amp;SortBy={sortBy}&amp;SortAscending={asc}&amp;PageSize={sortedSet.PageSize}";
            var expected =
                $"<div class=\"table-wrapper\"><table><thead><tr><th class=\"sortable\"><a href=\"{expectedUrl}\">{header}</a></th></tr></thead><tbody><tr><td>{cellInnerHtml}</td></tr></tbody></table></div>";

            var result = target.ToString();
            Assert.AreEqual(expected, result);
        }

        private void AssertRendersSortedWithDataProperty(RazorTable<Model> target, ISearchSet<Model> sortedSet,
            string sortBy, string dataProperty, string header, string cellInnerHtml)
        {
            if (target.Model == null || target.Model.Count() != 1)
            {
                Assert.Fail("This test requires that a model with exactly one item be set.");
            }

            // Expect this to be the opposite.
            var asc = !sortedSet.SortAscending;
            var expectedUrl =
                $"/?PageNumber={sortedSet.PageNumber}&amp;SortBy={sortBy}&amp;SortAscending={asc}&amp;PageSize={sortedSet.PageSize}";
            var expected =
                $"<div class=\"table-wrapper\"><table><thead><tr><th class=\"sortable\" data-property=\"{dataProperty}\"><a href=\"{expectedUrl}\">{header}</a></th></tr></thead><tbody><tr><td>{cellInnerHtml}</td></tr></tbody></table></div>";

            var result = target.ToString();
            Assert.AreEqual(expected, result);
        }

        #endregion

        #region Tests

        #region Constructor

        [TestMethod]
        public void TestConstructorSetsNewInstanceForColumnsProperty()
        {
            var one = new RazorTable<Model>((IEnumerable<Model>)null, null);
            var two = new RazorTable<Model>((IEnumerable<Model>)null, null);
            Assert.AreNotSame(one.Columns, two.Columns);
        }

        [TestMethod]
        public void TestConstructorSetsHtmlAttributesPropertyToEmptyDictionaryIfParameterIsNull()
        {
            Assert.IsNotNull(new RazorTable<Model>((IEnumerable<Model>)null, null).HtmlAttributes);
        }

        [TestMethod]
        public void TestConstructorSetsHtmlAttributesProperty()
        {
            var htmlAttributes = new {@class = "css-class"};
            Assert.AreEqual("css-class",
                new RazorTable<Model>((IEnumerable<Model>)null, htmlAttributes).HtmlAttributes["class"]);
        }

        [TestMethod]
        public void TestConstructorSetsModelToPaginatedResults()
        {
            Assert.IsNotNull(_sortedModels, "Test isn't setup correctly");
            Assert.AreSame(_models, _sortableTarget.Model);
        }

        [TestMethod]
        public void TestConstructorSetsIsSortableToTrueWhenPaginatedResultsConstructorIsUsed()
        {
            Assert.IsTrue(_sortableTarget.IsSortable);
            Assert.IsFalse(_target.IsSortable);
        }

        [TestMethod]
        public void TestConstructorSetsIsPagableToTrueWhenPaginatedResultsConstructorIsUsed()
        {
            Assert.IsTrue(_sortableTarget.IsPagable);
            Assert.IsFalse(_target.IsPagable);
        }

        [TestMethod]
        public void TestPaginatedConstructorSetsNullModelIfParameterIsNull()
        {
            var result = new RazorTable<Model>((ISearchSet<Model>)null, null);
            Assert.IsNull(result.Model);
        }

        [TestMethod]
        public void TestConstructorSetsIncludeHeadersForEmptyTableToTrueByDefault()
        {
            var result = new RazorTable<Model>((IEnumerable<Model>)null, null);
            Assert.IsTrue(result.IncludeHeadersForEmptyTable);
        }

        #endregion

        #region SortableTemplateColumnFor(Func<T, IHtmlString> template)

        [TestMethod]
        public void TestSortableTemplateColumnFor_OnlyHtmlStringParameter_RendersHeaderWithSortByAsTextAndTheTemplate()
        {
            _sortableTarget.Columns.Clear();
            _sortableTarget.SortableTemplateColumnFor("SortMe", x => new HtmlString("cool beans"));
            AssertRendersSorted(_sortableTarget, _sortedModels, "SortMe", "Sort Me", "cool beans");
        }

        #endregion

        #region SortableTemplateColumnFor(string headerText, Func<T, IHtmlString> template)

        [TestMethod]
        public void TestSortableTemplateColumnFor_HeaderNameAndHtmlStringParameter_RendersEscapedHeaderAndTheTemplate()
        {
            _sortableTarget.Columns.Clear();
            _sortableTarget.SortableTemplateColumnFor("R & D", "SortMe", x => new HtmlString("cool beans"));
            AssertRendersSorted(_sortableTarget, _sortedModels, "SortMe", "R &amp; D", "cool beans");
        }

        #endregion

        #region SortableTemplateColumnFor(Func<T, Func<T, HelperResult>> template)

        [TestMethod]
        public void TestSortableTemplateColumnFor_HelperResultOnlyParameter_RendersEmptyHeaderAndTheTemplate()
        {
            _sortableTarget.Columns.Clear();
            var helperResult = CreateHelperResult("cool beans");
            _sortableTarget.SortableTemplateColumnFor("SortMe", helperResult);
            AssertRendersSorted(_sortableTarget, _sortedModels, "SortMe", "Sort Me", "cool beans");
        }

        #endregion

        #region SortableTemplateColumnFor(string headerText, Func<T, Func<T, HelperResult>> template)

        [TestMethod]
        public void TestSortableTemplateColumnFor_HeaderNameAndHelperResult_RendersEscapedHeaderAndTheTemplate()
        {
            _sortableTarget.Columns.Clear();
            var helperResult = CreateHelperResult("cool beans");
            _sortableTarget.SortableTemplateColumnFor("R & D", "SortMe", helperResult);
            AssertRendersSorted(_sortableTarget, _sortedModels, "SortMe", "R &amp; D", "cool beans");
        }

        #endregion

        #region SortableTemplateColumnFor<TProperty>(Expression<Func<T, TProperty>> expression, Func<T, Func<T, HelperResult>> template)

        [TestMethod]
        public void
            TestSortableTemplateColumnFor_ExpressionHeaderNameAndHelperResult_RendersExpressionHeaderAndTheTemplate()
        {
            _sortableTarget.Columns.Clear();
            var helperResult = CreateHelperResult("cool beans");
            _sortableTarget.SortableTemplateColumnFor(x => x.Name, "SortMe", x => helperResult);
            AssertRendersSortedWithDataProperty(_sortableTarget, _sortedModels, "SortMe", "Name", "Name", "cool beans");
        }

        #endregion

        #region TemplateColumnFor(Func<T, IHtmlString> template)

        [TestMethod]
        public void TestTemplateColumnFor_OnlyHtmlStringParameter_RendersEmptyHeaderAndTheTemplate()
        {
            _target.Columns.Clear();
            _target.TemplateColumnFor(x => new HtmlString("cool beans"));
            AssertRenders(_target, null, "cool beans");
        }

        #endregion

        #region TemplateColumnFor(string headerText, Func<T, IHtmlString> template)

        [TestMethod]
        public void TestTemplateColumnFor_HeaderNameAndHtmlStringParameter_RendersEscapedHeaderAndTheTemplate()
        {
            _target.Columns.Clear();
            _target.TemplateColumnFor("R & D", x => new HtmlString("cool beans"));
            AssertRenders(_target, "R &amp; D", "cool beans");
        }

        #endregion

        #region TemplateColumnFor(Func<T, Func<T, HelperResult>> template)

        [TestMethod]
        public void TestTemplateColumnFor_HelperResultOnlyParameter_RendersEmptyHeaderAndTheTemplate()
        {
            _target.Columns.Clear();
            var helperResult = CreateHelperResult("cool beans");
            _target.TemplateColumnFor(helperResult);
            AssertRenders(_target, null, "cool beans");
        }

        #endregion

        #region TemplateColumnFor(string headerText, Func<T, Func<T, HelperResult>> template)

        [TestMethod]
        public void TestTemplateColumnFor_HeaderNameAndHelperResult_RendersEscapedHeaderAndTheTemplate()
        {
            _target.Columns.Clear();
            var helperResult = CreateHelperResult("cool beans");
            _target.TemplateColumnFor("R & D", helperResult);
            AssertRenders(_target, "R &amp; D", "cool beans");
        }

        #endregion

        #region TemplateColumnFor<TProperty>(Expression<Func<T, TProperty>> expression, Func<T, Func<T, HelperResult>> template)

        [TestMethod]
        public void TestTemplateColumnFor_ExpressionHeaderNameAndHelperResult_RendersExpressionHeaderAndTheTemplate()
        {
            _target.Columns.Clear();
            var helperResult = CreateHelperResult("cool beans");
            _target.TemplateColumnFor(x => x.Name, x => helperResult);
            AssertRendersWithDataProperty(_target, "Name", "Name", "cool beans");
        }

        [TestMethod]
        public void TestTemplateColumnFor_RendersFormTags()
        {
            _target.Columns.Clear();
            Func<Model, HelperResult> helperResult = (obj) => {
                return new HelperResult(x => {
                    // ReSharper disable once Mvc.ActionNotResolved
                    using (_htmlHelper.BeginForm())
                    {
                        x.Write("innards");
                    }
                });
            };
            _target.TemplateColumnFor(x => x.Name, x => helperResult);

            var result = _target.ToString();
            Assert.IsTrue(result.Contains("<td><form action=\"~/\" method=\"post\">innards</form></td>"));
        }

        #endregion

        #region ColumnFor<TProperty>(Expression<Func<T, TProperty>> expression)

        [TestMethod]
        public void TestColumnFor_ExpressionParameterOnly_RendersExpressionHeaderAndValueFromModel()
        {
            _target.Columns.Clear();
            _target.ColumnFor(x => x.Name);
            _modelInstance.Name = "Bilbo & Friends";
            AssertRendersWithDataProperty(_target, "Name", "Name", "Bilbo &amp; Friends");
        }

        #endregion

        #region ColumnFor<TProperty>(Expression<Func<T, TProperty>> expression, string headerText = null)

        [TestMethod]
        public void TestColumnFor_ExpressionParameterAndCustomHeader_RendersExpressionHeaderAndValueFromModel()
        {
            _target.Columns.Clear();
            _target.ColumnFor(x => x.Name, "Column Header");
            _modelInstance.Name = "Bilbo & Friends";
            AssertRenders(_target, "Column Header", "Bilbo &amp; Friends");
        }

        [TestMethod]
        public void TestColumnFor_ExpressionParameterAndCustomHeaderWithNullValue_RendersEmptyHeaderAndValueFromModel()
        {
            _target.Columns.Clear();
            _target.ColumnFor(x => x.Name, null);
            _modelInstance.Name = "Bilbo & Friends";
            AssertRenders(_target, null, "Bilbo &amp; Friends");
        }

        #endregion

        #region ColumnFor

        [TestMethod]
        public void TestColumnForReturnsSelf()
        {
            Assert.AreSame(_target, _target.ColumnFor(x => x.Name));
        }

        #endregion

        #region SortableColumnFor<TProperty>(Expression<Func<T, TProperty>> expression)

        [TestMethod]
        public void TestSortableColumnFor_ExpressionParameterOnly_RendersHeaderAndSortByUsingExpressionsPropertyName()
        {
            _sortableTarget.Columns.Clear();
            _sortableTarget.SortableColumnFor(x => x.Name);
            _modelInstance.Name = "Bilbo & Friends";
            AssertRendersSortedWithDataProperty(_sortableTarget, _sortedModels, "Name", "Name", "Name",
                "Bilbo &amp; Friends");
        }

        #endregion

        #region SortableColumnFor<TProperty>(Expression<Func<T, TProperty>> expression, string sortBy)

        [TestMethod]
        public void TestSortableColumnFor_ExpressionParameterAndSortByOnly_RendersExpressionHeaderAndValueFromModel()
        {
            _sortableTarget.Columns.Clear();
            _sortableTarget.SortableColumnFor(x => x.Name, "SortMe");
            _modelInstance.Name = "Bilbo & Friends";
            AssertRendersSortedWithDataProperty(_sortableTarget, _sortedModels, "SortMe", "Name", "Name",
                "Bilbo &amp; Friends");
        }

        #endregion

        #region SortableColumnFor<TProperty>(Expression<Func<T, TProperty>> expression, string headerText, string sortBy)

        [TestMethod]
        public void TestSortableColumnFor_ExpressionParameterAndCustomHeader_RendersExpressionHeaderAndValueFromModel()
        {
            _sortableTarget.Columns.Clear();
            _sortableTarget.SortableColumnFor(x => x.Name, "Column Header", "SortMe");
            _modelInstance.Name = "Bilbo & Friends";
            AssertRendersSorted(_sortableTarget, _sortedModels, "SortMe", "Column Header", "Bilbo &amp; Friends");
        }

        #endregion

        #region SortableColumnFor

        [TestMethod]
        public void TestSortableColumnForReturnsSelf()
        {
            Assert.AreSame(_sortableTarget, _sortableTarget.SortableColumnFor(x => x.Name, "SortMe"));
        }

        #endregion

        #region Render

        [TestMethod]
        public void TestRendersAnEmptyTableWhenTheModelIsNull()
        {
            InitializeForModel(null);
            Assert.AreEqual(
                "<div class=\"table-wrapper\"><table><thead><tr><th data-property=\"Name\">Name</th></tr></thead><tbody></tbody></table></div>",
                _target.ToString());
        }

        [TestMethod]
        public void TestRendersAnEmptyTableWhenTheModelHasNoItems()
        {
            InitializeForModel(new List<Model>());
            var controller = _pipeline.CreateAndInitializeController<FakeController>();
            var helper = _pipeline.CreateHtmlHelper(controller, (IEnumerable<Model>)null);
            _target.HtmlHelper = helper;
            Assert.AreEqual(
                "<div class=\"table-wrapper\"><table><thead><tr><th data-property=\"Name\">Name</th></tr></thead><tbody></tbody></table></div>",
                _target.ToString());
        }

        [TestMethod]
        public void
            TestRendersOnlyTheEmptyResultCaptionWhenTheModelIsNotNullAndHasNoItemsAndIncludeHeadersForEmptyTableIsFalse()
        {
            InitializeForModel(new List<Model>());
            var controller = _pipeline.CreateAndInitializeController<FakeController>();
            var helper = _pipeline.CreateHtmlHelper(controller, (IEnumerable<Model>)null);
            _target.HtmlHelper = helper;
            _target.EmptyResultCaption = "Neato";
            _target.IncludeHeadersForEmptyTable = false;
            _target.Caption = "Bad don't use this";
            Assert.AreEqual("<div class=\"table-wrapper\"><table><caption>Neato</caption><tbody></tbody></table></div>",
                _target.ToString());

            _target.IncludeHeadersForEmptyTable = true;

            Assert.AreEqual(
                "<div class=\"table-wrapper\"><table><caption>Neato</caption><thead><tr><th data-property=\"Name\">Name</th></tr></thead><tbody></tbody></table></div>",
                _target.ToString());
        }

        [TestMethod]
        public void TestRendersARowWithEmptyCellsWhenAModelItemIsNull()
        {
            _models.Clear();
            _models.Add(new Model {Name = "Cool"});
            _models.Add(null);
            Assert.AreEqual(
                "<div class=\"table-wrapper\"><table><thead><tr><th data-property=\"Name\">Name</th></tr></thead><tbody><tr><td>Cool</td></tr><tr><td></td></tr></tbody></table></div>",
                _target.ToString());
        }

        [TestMethod]
        public void TestRendersHtmlAttributesOnTable()
        {
            _models.Clear();
            _target.HtmlAttributes["class"] = "css-class";
            Assert.AreEqual(
                "<div class=\"table-wrapper\"><table class=\"css-class\"><thead><tr><th data-property=\"Name\">Name</th></tr></thead><tbody></tbody></table></div>",
                _target.ToString());
        }

        [TestMethod]
        public void TestRendersCaptionTagInsideTableTagWhenCaptionIsSet()
        {
            InitializeForModel(null);
            _target.WithCaption("Some caption you are!");
            Assert.AreEqual(
                "<div class=\"table-wrapper\"><table><caption>Some caption you are!</caption><thead><tr><th data-property=\"Name\">Name</th></tr></thead><tbody></tbody></table></div>",
                _target.ToString());
        }

        [TestMethod]
        public void TestRenderHtmlEscapesCaption()
        {
            InitializeForModel(null);
            _target.WithCaption("R & D");
            Assert.AreEqual(
                "<div class=\"table-wrapper\"><table><caption>R &amp; D</caption><thead><tr><th data-property=\"Name\">Name</th></tr></thead><tbody></tbody></table></div>",
                _target.ToString());
        }

        [TestMethod]
        public void TestRendersFooterIfFooterIsSet()
        {
            InitializeForModel(null);
            var footer = new Mock<IRazorTableFooter>();
            footer.Setup(x => x.Render(It.IsAny<HtmlHelper>(), It.IsAny<int>()))
                  .Returns(new TagBuilder("tfoot") {InnerHtml = "i am a foot"});
            _target.Footer = footer.Object;
            Assert.AreEqual(
                "<div class=\"table-wrapper\"><table><thead><tr><th data-property=\"Name\">Name</th></tr></thead><tbody></tbody></table><tfoot>i am a foot</tfoot></div>",
                _target.ToString());
        }

        [TestMethod]
        public void TestDoesNotRenderFooterIfFooterIsNull()
        {
            InitializeForModel(null);
            Assert.IsNull(_target.Footer, "Footer must be null for test to work");
            Assert.AreEqual(
                "<div class=\"table-wrapper\"><table><thead><tr><th data-property=\"Name\">Name</th></tr></thead><tbody></tbody></table></div>",
                _target.ToString());
        }

        [TestMethod]
        public void TestDoesNotRenderFooterIfFootersInnerHtmlIsNullOrEmpty()
        {
            InitializeForModel(null);
            var footer = new Mock<IRazorTableFooter>();

            foreach (var empties in new[] {string.Empty, null})
            {
                footer.Setup(x => x.Render(It.IsAny<HtmlHelper>(), It.IsAny<int>()))
                      .Returns(new TagBuilder("tfoot") {InnerHtml = empties});
                _target.Footer = footer.Object;
                Assert.AreEqual(
                    "<div class=\"table-wrapper\"><table><thead><tr><th data-property=\"Name\">Name</th></tr></thead><tbody></tbody></table></div>",
                    _target.ToString());
            }
        }

        [TestMethod]
        public void TestDoesNotRenderColumnIfColumnIsNotVisible()
        {
            InitializeForModel(null);
            _target.Columns.Single().IsVisible = false;

            Assert.AreEqual("<div class=\"table-wrapper\"><table><thead><tr></tr></thead><tbody></tbody></table></div>",
                _target.ToString());
        }

        [TestMethod]
        public void TestRendersAFooterRowIfAFooterCellHasBeenAddedToAColumn()
        {
            InitializeForModel(new List<Model>());
            _target.WithFooterCell("a value of a foot");
            var controller = _pipeline.CreateAndInitializeController<FakeController>();
            var helper = _pipeline.CreateHtmlHelper(controller, (IEnumerable<Model>)null);
            _target.HtmlHelper = helper;
            
            Assert.AreEqual(
                "<div class=\"table-wrapper\"><table><thead><tr><th data-property=\"Name\">Name</th></tr></thead><tbody></tbody><tfoot><tr><td>a value of a foot</td></tr></tfoot></table></div>",
                _target.ToString());
        }

        #endregion

        #region WithCaption

        [TestMethod]
        public void TestWithCaptionReturnsRazorTableInstance()
        {
            Assert.AreSame(_target, _target.WithCaption("blah"));
        }

        [TestMethod]
        public void TestWithCaptionCanBeSetToNullOrEmptyOrWhiteSpace()
        {
            var emptyishStrings = new[] {string.Empty, null, "   "};
            foreach (var empty in emptyishStrings)
            {
                _target.WithCaption(empty);
                Assert.AreEqual(empty, _target.Caption);
            }
        }

        #endregion

        #region WithPaginatedFooter

        [TestMethod]
        public void TestWithPaginatedFooterThrowsExceptionIfModelIsNotPagable()
        {
            Assert.IsFalse(_target.IsPagable, "Sanity");
            MyAssert.Throws<InvalidOperationException>(() => _target.WithPaginatedFooter());

            _sortedModels.EnablePaging = false;
            _sortableTarget = new RazorTable<Model>(_sortedModels, null);
            MyAssert.Throws<InvalidOperationException>(() => _sortableTarget.WithPaginatedFooter());
        }

        [TestMethod]
        public void TestWithPaginatedFooterSetsFooterPropertyToNewInstanceOfRazorTablePaginatedFooter()
        {
            Assert.IsNull(_sortableTarget.Footer);
            _sortableTarget.WithPaginatedFooter();
            Assert.IsInstanceOfType(_sortableTarget.Footer, typeof(RazorTablePaginatedFooter));
        }

        [TestMethod]
        public void TestWithPaginatedFooterSetsShowPageSizeOptionsToExpectedValues()
        {
            _sortableTarget.WithPaginatedFooter();
            var result = (RazorTablePaginatedFooter)_sortableTarget.Footer;
            Assert.IsTrue(result.ShowPageSizeOptions, "This should be true by default if no parameter is given.");

            _sortableTarget.WithPaginatedFooter(true);
            result = (RazorTablePaginatedFooter)_sortableTarget.Footer;
            Assert.IsTrue(result.ShowPageSizeOptions);

            _sortableTarget.WithPaginatedFooter(false);
            result = (RazorTablePaginatedFooter)_sortableTarget.Footer;
            Assert.IsFalse(result.ShowPageSizeOptions);
        }

        #endregion

        #region IsVisible

        [TestMethod]
        public void TestIsVisibleSetsIsVisiblePropertyOnLastAddedColumn()
        {
            _target.ColumnFor(x => x.Name);
            _target.ColumnFor(x => x.Name);
            var first = _target.Columns.First();
            var second = _target.Columns.Last();
            Assert.AreNotSame(first, second, "sanity");

            Assert.IsTrue(first.IsVisible);
            Assert.IsTrue(second.IsVisible);

            _target.IsVisible(false);

            Assert.IsTrue(first.IsVisible);
            Assert.IsFalse(second.IsVisible);
        }

        #endregion

        #region WithCellBuilder

        [TestMethod]
        public void TestWithCellBuilderThrowsInvalidOperationExceptionIfCalledBeforeAColumnExists()
        {
            InitializeForModel(null);
            _target.Columns.Clear(); // Initializing adds a column, so needs to be cleared
            MyAssert.Throws<InvalidOperationException>(() => _target.WithCellBuilder(null));
        }

        [TestMethod]
        public void TestWithCellBuilderCorrectlyAddsTheActionToTheLastAddedRazorColumn()
        {
            Action<Model, TagBuilder> cb1 = (x, tag) => { };
            Action<Model, TagBuilder> cb2 = (x, tag) => { };

            InitializeForModel(null);

            _target.ColumnFor(x => x.Name).WithCellBuilder(cb1);
            var column1 = _target.Columns.Last();
            _target.ColumnFor(x => x.Name).WithCellBuilder(cb2);
            var column2 = _target.Columns.Last();

            Assert.AreSame(cb1, column1.CellBuilder);
            Assert.AreSame(cb2, column2.CellBuilder);
        }

        #endregion

        #endregion

        #region Helper classes

        private class Model
        {
            public string Name { get; set; }
        }

        private class TestPagingSearchSet : ISearchSet<Model>
        {
            public int PageNumber { get; set; }
            public int PageSize { get; set; }
            public int PageCount { get; set; }
            public int Count { get; set; }
            public bool EnablePaging { get; set; }
            public string SortBy { get; set; }
            public bool SortAscending { get; set; }
            public string DefaultSortBy { get; }
            public bool DefaultSortAscending { get; }
            public List<string> ExportableProperties { get; set; }

            public void ModifyValues(ISearchMapper mapper)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<Model> Results { get; set; }
        }

        #endregion
    }
}
