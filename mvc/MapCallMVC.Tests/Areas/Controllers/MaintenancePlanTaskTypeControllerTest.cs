using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Production.Controllers;
using MapCallMVC.Areas.Production.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class MaintenancePlanTaskTypeControllerTest : MapCallMvcControllerTestBase<MaintenancePlanTaskTypeController, MaintenancePlanTaskType>
    {
        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            const RoleModules role = RoleModules.ProductionDataAdministration;
            Authorization.Assert(auth => {
                auth.RequiresRole("~/Production/MaintenancePlanTaskType/Show", role, RoleActions.Read);
                auth.RequiresRole("~/Production/MaintenancePlanTaskType/Search", role, RoleActions.Read);
                auth.RequiresRole("~/Production/MaintenancePlanTaskType/Index", role, RoleActions.Read);
                auth.RequiresRole("~/Production/MaintenancePlanTaskType/New", role, RoleActions.Add);
                auth.RequiresRole("~/Production/MaintenancePlanTaskType/Create", role, RoleActions.Add);
                auth.RequiresRole("~/Production/MaintenancePlanTaskType/Edit", role, RoleActions.Edit);
                auth.RequiresRole("~/Production/MaintenancePlanTaskType/Update", role, RoleActions.Edit);
                auth.RequiresRole("~/Production/MaintenancePlanTaskType/Destroy", role, RoleActions.Delete);
            });
        }

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<MaintenancePlanTaskType>().Create(new { Description = "Test Type 1", IsActive = true });
            var entity1 = GetEntityFactory<MaintenancePlanTaskType>().Create(new { Description = "Test Type 2", IsActive = true });
            var search = new SearchMaintenancePlanTaskType();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
            }
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var model = GetEntityFactory<MaintenancePlanTaskType>().Create();
            var expected = "Updated Task Type";

            _target.Update(_viewModelFactory.BuildWithOverrides<MaintenancePlanTaskTypeViewModel, MaintenancePlanTaskType>(model, new {
                Description = expected
            }));

            Assert.AreEqual(expected, Session.Get<MaintenancePlanTaskType>(model.Id).Description);
        }

        #endregion

        #endregion
    }
}