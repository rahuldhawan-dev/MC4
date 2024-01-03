using System.Collections.Generic;
using System.Linq;
using Historian.Data.Client.Entities;
using Historian.Data.Client.Repositories;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Facilities.Controllers;
using MapCallMVC.Areas.Facilities.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Facilities.Controllers
{
    [TestClass]
    public class ScadaReadingControllerTest : MapCallMvcControllerTestBase<ScadaReadingController, ScadaTagName>
    {
        #region Private Members

        private Mock<IRawDataRepository> _rawDataRepo;

        #endregion

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            i.For<IRawDataRepository>().Use((_rawDataRepo = new Mock<IRawDataRepository>()).Object);
            base.InitializeObjectFactory(i);
        }

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                var module = RoleModules.ProductionEquipment;
                a.RequiresRole("~/Facilities/ScadaReading/Search/", module, RoleActions.Read);
                a.RequiresRole("~/Facilities/ScadaReading/Index/", module, RoleActions.Read);
            });
        }

        [TestMethod]
        public void TestIndexRedirectsBackToSearchIfNoTagNameProvided()
        {
            var result = _target.Index(new SearchScadaReading());

            MvcAssert.RedirectsToRoute(result, "Search");
        }

        [TestMethod]
        public void TestIndexRedirectsBackToSearchIfTagNameHasNoAssociatedReadings()
        {
            var tagName = GetEntityFactory<ScadaTagName>().Create(new {TagName = "foo"});

            _rawDataRepo.Setup(x => x.FindByTagName("foo", false, null, null)).Returns(Enumerable.Empty<RawData>());

            var result = _target.Index(new SearchScadaReading {TagName = tagName.Id});

            MvcAssert.RedirectsToRoute(result, "Search");
        }

        [TestMethod]
        public void TestIndexRedirectsBackToSearchIfTagNameWithGivenIdNotFound()
        {
            var result = _target.Index(new SearchScadaReading {TagName = 1});

            MvcAssert.RedirectsToRoute(result, "Search");
        }

        [TestMethod]
        public void TestIndexRendersExcelFileIfThatsTheKindaThingYoureInto()
        {
            var tagName = GetEntityFactory<ScadaTagName>().Create(new {TagName = "foo"});
            var search = new SearchScadaReading {TagName = tagName.Id};
            var readings = new List<RawData> {new RawData()};

            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;
            _rawDataRepo.Setup(x => x.FindByTagName("foo", false, null, null)).Returns(readings);

            var result = _target.Index(new SearchScadaReading {TagName = 1});

            MvcAssert.RespondsWithExcelResult(result);
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search model doesn't implement ISearchSet
            var tagName = GetEntityFactory<ScadaTagName>().Create(new {TagName = "foo"});
            var search = new SearchScadaReading {TagName = tagName.Id};
            var readings = new List<RawData> {new RawData()};

            _rawDataRepo.Setup(x => x.FindByTagName("foo", false, null, null)).Returns(readings);

            var result = _target.Index(search);

            MvcAssert.IsViewNamed(result, "Index");
            MvcAssert.IsViewWithModel(result, search);
            Assert.AreEqual(tagName, search.TagNameObj);
            Assert.AreEqual(readings, search.Results);
        }

        [TestMethod]
        public override void TestIndexRedirectsToSearchIfModelStateIsInvalid()
        {
            Assert.Inconclusive("Implement correctly and test me.");
        }

        [TestMethod]
        public override void TestIndexRedirectsToSearchIfThereAreZeroResults()
        {
            Assert.Inconclusive("Test me");
        }

        [TestMethod]
        public override void TestSearchReturnsSearchViewWithModel()
        {
            // Need to override this test because the search action doesn't
            // use ActionHelper and the search model doesn't implement ISearchSet.

            MvcAssert.IsViewNamed(_target.Search(null), "Search");
        }

        [TestMethod]
        public override void TestIndexCanPerformSearchForAllSearchModelProperties()
        {
            // noop. Search model doesn't implement ISearchSet or use ActionHelper.
            // Other Index tests cover this.
        }
    }
}