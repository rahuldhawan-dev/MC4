using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Facilities.Controllers;
using MapCallMVC.Areas.Facilities.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Facilities.Controllers
{
    [TestClass]
    public class EmergencyResponsePlanControllerTest : MapCallMvcControllerTestBase<EmergencyResponsePlanController, EmergencyResponsePlan, EmergencyResponsePlanRepository>
    {
        #region Init/Cleanup

        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        #endregion

        #region Authorization

        [TestMethod]		
        public override void TestControllerAuthorization()
        {
            var role = RoleModules.ProductionEquipment;

            Authorization.Assert(a => {
                a.RequiresRole("~/Facilities/EmergencyResponsePlan/Search/", role);
                a.RequiresRole("~/Facilities/EmergencyResponsePlan/Show/", role);
                a.RequiresRole("~/Facilities/EmergencyResponsePlan/Index/", role);
                a.RequiresRole("~/Facilities/EmergencyResponsePlan/New/", role, RoleActions.Add);
                a.RequiresRole("~/Facilities/EmergencyResponsePlan/Create/", role, RoleActions.Add);
                a.RequiresRole("~/Facilities/EmergencyResponsePlan/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/Facilities/EmergencyResponsePlan/Update/", role, RoleActions.Edit);
                a.RequiresRole("~/Facilities/EmergencyResponsePlan/Destroy/", role, RoleActions.Delete);
			});
		}				

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<EmergencyResponsePlan>().Create(new {Description = "description 0"});
            var entity1 = GetEntityFactory<EmergencyResponsePlan>().Create(new {Description = "description 1"});
            var search = new SearchEmergencyResponsePlan();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.Description, "Description");
                helper.AreEqual(entity1.Description, "Description", 1);
            }
        }

        #endregion

        #region New

        [TestMethod]
        public void TestNewOnlyIncludesActiveOperatingCentersInLookupData()
        {
            var activeOpc = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsActive = true });
            var inactiveOpc = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsActive = false });

            _target.New();

            var opcData = (IEnumerable<SelectListItem>)_target.ViewData["OperatingCenter"];
            Assert.IsTrue(opcData.Any(x => x.Value == activeOpc.Id.ToString()));
            Assert.IsFalse(opcData.Any(x => x.Value == inactiveOpc.Id.ToString()));
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<EmergencyResponsePlan>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditEmergencyResponsePlan, EmergencyResponsePlan>(eq, new {
                Description = expected
            }));

            Assert.AreEqual(expected, Session.Get<EmergencyResponsePlan>(eq.Id).Description);
        }

        #endregion
    }
}
