using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.WaterQuality.Controllers;
using MapCallMVC.Areas.WaterQuality.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.WaterQuality.Controllers
{
    [TestClass]
    public class WaterSampleControllerTest : MapCallMvcControllerTestBase<WaterSampleController, WaterSample>
    {
        #region Init

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateWaterSample)vm;
                model.SampleIdMatrix = GetEntityFactory<SampleIdMatrix>().Create().Id;
                model.UnitOfMeasure = GetEntityFactory<UnitOfWaterSampleMeasure>().Create().Id;
                model.SampleDate = DateTime.Now;
            };
            options.InitializeUpdateViewModel = (vm) => {
                var model = (EditWaterSample)vm;
                model.SampleIdMatrix = GetEntityFactory<SampleIdMatrix>().Create().Id;
                model.SampleDate = DateTime.Now;
            };
        }

        #endregion

        #region Authorization

        [TestMethod]		
        public override void TestControllerAuthorization()
        {
            var role = WaterSampleController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/WaterQuality/WaterSample/Search/", role);
                a.RequiresRole("~/WaterQuality/WaterSample/Show/", role);
                a.RequiresRole("~/WaterQuality/WaterSample/Index/", role);
                a.RequiresRole("~/WaterQuality/WaterSample/New/", role, RoleActions.Add);
                a.RequiresRole("~/WaterQuality/WaterSample/Create/", role, RoleActions.Add);
                a.RequiresRole("~/WaterQuality/WaterSample/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/WaterQuality/WaterSample/Update/", role, RoleActions.Edit);
                a.RequiresRole("~/WaterQuality/WaterSample/Destroy/", role, RoleActions.Delete);
			});
		}				

        #endregion
                
        #region New

        [TestMethod]
        public void TestNewOnlyIncludesActiveOperatingCentersInLookupData()
        {
            var activeOpc = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsActive = true });
            var inactiveOpc = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsActive = false });

            _target.New(null);

            var opcData = (IEnumerable<SelectListItem>)_target.ViewData["OperatingCenter"];
            Assert.IsTrue(opcData.Any(x => x.Value == activeOpc.Id.ToString()));
            Assert.IsFalse(opcData.Any(x => x.Value == inactiveOpc.Id.ToString()));
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<WaterSample>().Create(new {AnalysisPerformedBy = "description 0"});
            var entity1 = GetEntityFactory<WaterSample>().Create(new {AnalysisPerformedBy = "description 1"});
            var search = new SearchWaterSample();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "WaterSampleID");
                helper.AreEqual(entity1.Id, "WaterSampleID", 1);
                helper.AreEqual(entity0.AnalysisPerformedBy, "AnalysisPerformedBy");
                helper.AreEqual(entity1.AnalysisPerformedBy, "AnalysisPerformedBy", 1);
            }
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<WaterSample>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditWaterSample, WaterSample>(eq, new {
                AnalysisPerformedBy = expected
            }));

            Assert.AreEqual(expected, Session.Get<WaterSample>(eq.Id).AnalysisPerformedBy);
        }

        #endregion
    }
}
