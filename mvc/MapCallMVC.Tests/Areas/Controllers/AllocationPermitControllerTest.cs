using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Environmental.Controllers;
using MapCallMVC.Areas.Environmental.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Controllers;
using MMSINC.Data;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Controllers
{
    [TestClass]
    public class AllocationPermitControllerTest : MapCallMvcControllerTestBase<AllocationPermitController, AllocationPermit>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<IFacilityRepository>().Use<FacilityRepository>();
        }

        #endregion

        #region Roles

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                a.RequiresRole("~/Environmental/AllocationPermit/Show/", RoleModules.EnvironmentalGeneral, RoleActions.Read);
                a.RequiresRole("~/Environmental/AllocationPermit/Search/", RoleModules.EnvironmentalGeneral, RoleActions.Read);
                a.RequiresRole("~/Environmental/AllocationPermit/Index/", RoleModules.EnvironmentalGeneral, RoleActions.Read);
                a.RequiresRole("~/Environmental/AllocationPermit/Edit/", RoleModules.EnvironmentalGeneral, RoleActions.Edit);
                a.RequiresRole("~/Environmental/AllocationPermit/Update/", RoleModules.EnvironmentalGeneral, RoleActions.Edit);
                a.RequiresRole("~/Environmental/AllocationPermit/New/", RoleModules.EnvironmentalGeneral, RoleActions.Add);
                a.RequiresRole("~/Environmental/AllocationPermit/Create/", RoleModules.EnvironmentalGeneral, RoleActions.Add);
                a.RequiresRole("~/Environmental/AllocationPermit/Destroy/", RoleModules.EnvironmentalGeneral, RoleActions.Delete);
            });
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = _container.GetInstance<TestDataFactory<AllocationPermit>>().Create(new { PermitNotes = "notes 0" });
            var entity1 = _container.GetInstance<TestDataFactory<AllocationPermit>>().Create(new { PermitNotes = "notes 1" });
            var search = new SearchAllocationPermit();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "AllocationGroupingID");
                helper.AreEqual(entity1.Id, "AllocationGroupingID", 1);
                helper.AreEqual(entity0.PermitNotes, "Notes");
                helper.AreEqual(entity1.PermitNotes, "Notes", 1);
            }
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var permit = _container.GetInstance<TestDataFactory<EnvironmentalPermit>>().Create();
            var eq = _container.GetInstance<TestDataFactory<AllocationPermit>>().Create(new { EnvironmentalPermit = permit});
            var expected = "notes field";
            var entity = _viewModelFactory.BuildWithOverrides<EditAllocationPermit, AllocationPermit>(eq, new {
                PermitNotes = expected
            });
            var result = _target.Update(entity) as RedirectToRouteResult;

            Assert.AreEqual("Show", result.RouteValues["action"]);
            Assert.AreEqual(expected, Session.Get<AllocationPermit>(eq.Id).PermitNotes);
        }

        #endregion

        #region LookupData

        [TestMethod]
        public void TestSetLookUpDataForEnvironmentalPermitsSetsCorrectlyOnNew()
        {
            var environmentalPermitType1 = GetEntityFactory<EnvironmentalPermitType>().Create();
            var environmentalPermitType2 = GetEntityFactory<EnvironmentalPermitType>().Create();
            var environmentalPermitList = GetEntityFactory<EnvironmentalPermit>().CreateList(5, new {EnvironmentalPermitType = environmentalPermitType1});
            var environmentalAllocationPermit = GetEntityFactory<EnvironmentalPermit>().Create(new {EnvironmentalPermitType = environmentalPermitType2 });


            _target.SetLookupData(ControllerAction.New);

            var environmentalPermits = (IEnumerable<SelectListItem>)_target.ViewData["EnvironmentalPermit"];

            Assert.AreNotEqual(environmentalPermitList.Count, environmentalPermits.Count());
            Assert.AreEqual(environmentalAllocationPermit.Id.ToString(), environmentalPermits.First().Value);
        }

        #endregion
    }
}
