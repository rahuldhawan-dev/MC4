using System;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Facilities.Controllers;
using MapCallMVC.Areas.Facilities.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Facilities.Controllers
{
    [TestClass]
    public class InterconnectionTestControllerTest : MapCallMvcControllerTestBase<InterconnectionTestController, InterconnectionTest>
    {
        #region Init

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateInterconnectionTest)vm;
                model.State = GetEntityFactory<State>().Create().Id;
                model.OperatingCenter = GetEntityFactory<OperatingCenter>().Create().Id;
                model.Facility = GetEntityFactory<Facility>().Create().Id;
                model.InterconnectionInspectionRating = GetEntityFactory<InterconnectionInspectionRating>().Create().Id;
                model.Employee = GetEntityFactory<Employee>().Create().Id;
                model.InspectionDate = DateTime.Now;
            };
            options.InitializeUpdateViewModel = (vm) => {
                var model = (EditInterconnectionTest)vm;
                model.State = GetEntityFactory<State>().Create().Id;
                model.OperatingCenter = GetEntityFactory<OperatingCenter>().Create().Id;
                model.Facility = GetEntityFactory<Facility>().Create().Id;
                model.InterconnectionInspectionRating = GetEntityFactory<InterconnectionInspectionRating>().Create().Id;
                model.Employee = GetEntityFactory<Employee>().Create().Id;
                model.InspectionDate = DateTime.Now;
            };
        }

        #endregion
        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = InterconnectionTestController.Role;

            Authorization.Assert(a =>
            {
                a.RequiresRole("~/FieldOperations/InterconnectionTest/Search/", role);
                a.RequiresRole("~/FieldOperations/InterconnectionTest/Show/", role);
                a.RequiresRole("~/FieldOperations/InterconnectionTest/Index/", role);
                a.RequiresRole("~/FieldOperations/InterconnectionTest/New/", role, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/InterconnectionTest/Create/", role, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/InterconnectionTest/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/InterconnectionTest/Update/", role, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/InterconnectionTest/Destroy", role, RoleActions.Delete);
            });
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<InterconnectionTest>().Create(new { RepresentativeOnSite = "Sunil" });
            var entity1 = GetEntityFactory<InterconnectionTest>().Create(new { RepresentativeOnSite = "Sai Sunil" });
            var search = new SearchInterconnectionTest();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.RepresentativeOnSite, "RepresentativeOnSite");
                helper.AreEqual(entity1.RepresentativeOnSite, "RepresentativeOnSite", 1);
            }
        }

        #endregion

        #region Show

        [TestMethod]
        public void TestShowRespondsToFrag()
        {
            var interconnectionTest = GetEntityFactory<InterconnectionTest>().Create();
            InitializeControllerAndRequest("~/FieldOperations/InterconnectionTest/Show/" + interconnectionTest.Id + ".frag");

            var result = _target.Show(interconnectionTest.Id) as PartialViewResult;

            MvcAssert.IsViewNamed(result, "_ShowPopup");
            MvcAssert.IsViewWithModel(result, interconnectionTest);
        }

        #endregion
    }
}
