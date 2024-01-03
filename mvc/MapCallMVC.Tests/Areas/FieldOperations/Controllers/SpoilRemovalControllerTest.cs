using System;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using MMSINC.Results;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class SpoilRemovalControllerTest : MapCallMvcControllerTestBase<SpoilRemovalController, SpoilRemoval>
    {
        #region Init

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateSpoilRemoval)vm;
                model.State = GetEntityFactory<State>().Create().Id;
                model.OperatingCenter = GetEntityFactory<OperatingCenter>().Create().Id;
            };
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresRole("~/FieldOperations/SpoilRemoval/Edit/", RoleModules.FieldServicesWorkManagement, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/SpoilRemoval/Update/", RoleModules.FieldServicesWorkManagement, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/SpoilRemoval/New/", RoleModules.FieldServicesWorkManagement, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/SpoilRemoval/Create/", RoleModules.FieldServicesWorkManagement, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/SpoilRemoval/Index/", RoleModules.FieldServicesWorkManagement, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/SpoilRemoval/Show/", RoleModules.FieldServicesWorkManagement, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/SpoilRemoval/Search/", RoleModules.FieldServicesWorkManagement, RoleActions.UserAdministrator);
            });
        }

        #region Index

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because the search model has required properties that
            // the auto-tester can't set with expected values. This means no
            // results are returned.
            var eq1 = GetFactory<SpoilRemovalFactory>().Create();
            var eq2 = GetFactory<SpoilRemovalFactory>().Create();
            var search = new SearchSpoilRemoval();

            var result = _target.Index(search) as ViewResult;
            var resultModel = ((SearchSpoilRemoval)result.Model).Results.ToList();

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(2, resultModel.Count);
            Assert.AreSame(eq1, resultModel[0]);
            Assert.AreSame(eq2, resultModel[1]);
        }

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var locations = GetFactory<SpoilStorageLocationFactory>().CreateList(2);
            var entity0 = GetFactory<SpoilRemovalFactory>().Create(new { RemovedFrom = locations[0]});
            var entity1 = GetFactory<SpoilRemovalFactory>().Create(new { RemovedFrom = locations[1]});
            var search = new SearchSpoilRemoval();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(locations[0], "RemovedFrom");
                helper.AreEqual(locations[1], "RemovedFrom", 1);
            }
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var st = GetEntityFactory<State>().Create();
            var oc = GetEntityFactory<OperatingCenter>().Create(new {State = st});
           
            var sl = GetEntityFactory<SpoilStorageLocation>().Create(new {OperatingCenter = oc});
           
            var fl = GetEntityFactory<SpoilFinalProcessingLocation>().Create();
            var eq = GetEntityFactory<SpoilRemoval>().Create(new {RemovedFrom = sl, FinalDestination = fl});
            DateTime expected = System.DateTime.Today.Date;
            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditSpoilRemoval, SpoilRemoval>(eq, new {
                DateRemoved = expected
            }));
            Assert.AreEqual(expected, Session.Get<SpoilRemoval>(eq.Id).DateRemoved);
        }

        #endregion
    }
}
