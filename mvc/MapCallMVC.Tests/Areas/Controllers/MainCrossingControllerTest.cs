using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Configuration;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Facilities.Controllers;
using MapCallMVC.Areas.Facilities.Models.ViewModels;
using MapCallMVC.Areas.Reports.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using MMSINC.Controllers;
using MMSINC.Data;
using MMSINC.Results;
using MMSINC.Testing;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Controllers
{
    [TestClass]
    public class MainCrossingControllerTest : MapCallMvcControllerTestBase<MainCrossingController, MainCrossing, MainCrossingRepository>
    {
        #region Private Members

        #endregion
        
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<ITownRepository>().Use<TownRepository>();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<IRecurringFrequencyUnitRepository>().Use<RecurringFrequencyUnitRepository>();
        }

        #endregion

        #region Roles

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var module = RoleModules.FieldServicesAssets;
                a.RequiresRole("~/Facilities/MainCrossing/Show/", module, RoleActions.Read);
                a.RequiresRole("~/Facilities/MainCrossing/Search/", module, RoleActions.Read);
                a.RequiresRole("~/Facilities/MainCrossing/Index/", module, RoleActions.Read);
                a.RequiresRole("~/Facilities/MainCrossing/ByTownIdForWorkOrders/", module, RoleActions.Read);
                a.RequiresRole("~/Facilities/MainCrossing/ByOperatingCenterId/", module, RoleActions.Read);
                a.RequiresRole("~/Facilities/MainCrossing/ByOperatingCenterIds/", module, RoleActions.Read);
                a.RequiresRole("~/Facilities/MainCrossing/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/Facilities/MainCrossing/Update/", module, RoleActions.Edit);
                a.RequiresRole("~/Facilities/MainCrossing/New/", module, RoleActions.Add);
                a.RequiresRole("~/Facilities/MainCrossing/Create/", module, RoleActions.Add);
                a.RequiresSiteAdminUser("~/Facilities/MainCrossing/Destroy/");
            });
        }

        #endregion

        #region Index

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            var mainCrossingStatusEntity = GetEntityFactory<MainCrossingStatus>().Create();
            var crossingCategoryEntity = GetEntityFactory<CrossingCategory>().Create();
            var entity0 = GetFactory<MainCrossingFactory>().Create(new {
                MainCrossingStatus = mainCrossingStatusEntity
            });
            var entity1 = GetFactory<MainCrossingFactory>().Create(new {
                CrossingCategory = crossingCategoryEntity
            });
            _target.ControllerContext = new ControllerContext();

            var search = new SearchMainCrossing();

            var result = _target.Index(search);
            
            Assert.AreEqual(2, search.Count);

            search.MainCrossingStatus = mainCrossingStatusEntity.Id;

            result = _target.Index(search);

            Assert.AreEqual(1, search.Count);
            Assert.AreEqual(entity0.Id, search.Results.First().Id);

            search.CrossingCategory = crossingCategoryEntity.Id;
            search.MainCrossingStatus = null;

            _ = _target.Index(search);

            Assert.AreEqual(1, search.Count);
            Assert.AreEqual(entity1.Id, search.Results.First().Id);
        }

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetFactory<MainCrossingFactory>().Create();
            var entity1 = GetFactory<MainCrossingFactory>().Create();
            var search = new SearchMainCrossing();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                //helper.AreEqual(entity0.OperatingCenter, "Operating Center");
                //helper.AreEqual(entity1.OperatingCenter, "Operating Center", 1);
            }
        }

        [TestMethod]
        public void TestIndexRespondsToMapWithExpectedModels()
        {
            InitializeControllerAndRequest("~/Facilities/MainCrossing/Index.map");
            var good = GetFactory<MainCrossingFactory>().Create(new { OperatingCenter = typeof(UniqueOperatingCenterFactory) });
            var bad = GetFactory<MainCrossingFactory>().Create(new { OperatingCenter = typeof(UniqueOperatingCenterFactory) });
            var model = new SearchMainCrossing
            {
                OperatingCenter = new[] {good.OperatingCenter.Id}
            };
            var result = (MapResult)_target.Index(model);

            var resultModel = result.CoordinateSets.Single().Coordinates.Single();
            Assert.AreEqual(good.Id, resultModel.Id);
        }

        #endregion

        #region Cascades

        [TestMethod]
        public void TestByOperatingCenterIdGetsMainCrossingsByOperatingCenterIdAsDisplayItems()
        {
            var wrongOp = GetFactory<UniqueOperatingCenterFactory>().Create();
            var rightOp = GetFactory<UniqueOperatingCenterFactory>().Create();

            var inRightOp = GetEntityFactory<MainCrossing>().CreateArray(2, new {OperatingCenter = rightOp, Street = (Street)null, ClosestCrossStreet = (Street)null });
            var inWrongOp = GetEntityFactory<MainCrossing>().CreateArray(2, new {OperatingCenter = wrongOp});

            var result = (CascadingActionResult)_target.ByOperatingCenterId(rightOp.Id);
            var resultData = (List<MainCrossingDisplayItem>)result.Data;

            foreach (var datum in resultData)
            {
                Assert.AreEqual(rightOp.OperatingCenterCode, datum.OperatingCenter);
                Assert.AreEqual($"CR{datum.Id} - {datum.OperatingCenter} - {datum.Town} - {datum.MainCrossingStatus}",
                    datum.Display);
            }
        }

        [TestMethod]
        public void TestByOperatingCenterIdsGetsMainCrossingsByOperatingCenterIdsAsDisplayItems()
        {
            var rightOps = GetFactory<UniqueOperatingCenterFactory>().CreateArray(2);
            var wrongOps = GetFactory<UniqueOperatingCenterFactory>().CreateArray(2);

            var inRightOp = GetEntityFactory<MainCrossing>()
                           .CreateArray(2, new {OperatingCenter = rightOps[0], Street = (Street)null, ClosestCrossStreet = (Street)null})
                           .Union(GetEntityFactory<MainCrossing>()
                               .CreateArray(2, new {OperatingCenter = rightOps[1], Street = (Street)null, ClosestCrossStreet = (Street)null}));
            var inWrongOp = GetEntityFactory<MainCrossing>()
                           .CreateArray(2, new {OperatingCenter = wrongOps[0], Street = (Street)null, ClosestCrossStreet = (Street)null})
                           .Union(GetEntityFactory<MainCrossing>()
                               .CreateArray(2, new {OperatingCenter = wrongOps[1], Street = (Street)null, ClosestCrossStreet = (Street)null}));

            var result = (CascadingActionResult)_target.ByOperatingCenterIds(rightOps.Select(o => o.Id).ToArray());
            var resultData = (List<MainCrossingDisplayItem>)result.Data;

            foreach (var datum in resultData)
            {
                Assert.IsTrue(rightOps.Select(o => o.OperatingCenterCode).Contains(datum.OperatingCenter));
                Assert.AreEqual($"CR{datum.Id} - {datum.OperatingCenter} - {datum.Town} - {datum.MainCrossingStatus}",
                    datum.Display);
            }
        }

        #endregion

        #region Lookup Data

        [TestMethod]
        public void TestSetLookUpDataForOperatingCenterSetsCorrectlyOnNew()
        {
            var opc1 = GetEntityFactory<OperatingCenter>().CreateList(5);
            var opc2 = GetEntityFactory<OperatingCenter>().Create(new { IsActive = true });

            _target.SetLookupData(ControllerAction.New);

            var opcs = (IEnumerable<SelectListItem>)_target.ViewData["OperatingCenter"];

            Assert.AreNotEqual(opc1.Count, opcs.Count());
            Assert.AreEqual(1, opcs.Count());
            Assert.IsTrue(opc2.Id.ToString() == opcs.First().Value);
        }

        #endregion
    }
}
