using System;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Reports.Controllers;
using MapCallMVC.Areas.Reports.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Reports.Controllers
{
    [TestClass]
    public class MainBreakPowerPlantControllerTest : MapCallMvcControllerTestBase<MainBreakPowerPlantController, MainBreak>
    {
        #region Init/Cleanup
        
        protected override User CreateUser()
        {
            var user = base.CreateUser();
            user.IsAdmin = true;
            return user;
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = () => {
                var mainBreakWorkDescription = GetFactory<WaterMainBreakReplaceWorkDescriptionFactory>().Create();
                var validWorkOrder = GetEntityFactory<WorkOrder>().Create(new { WorkDescription = mainBreakWorkDescription, ApprovedOn = DateTime.Now });
                return GetFactory<MainBreakFactory>().Create(new { WorkOrder = validWorkOrder });
            };
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var role = RoleModules.FieldServicesWorkManagement;
                a.RequiresRole("~/Reports/MainBreakPowerPlant/Index", role);
                a.RequiresRole("~/Reports/MainBreakPowerPlant/Search", role);
            });
        }

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var mainBreakWorkDescription = GetFactory<WaterMainBreakReplaceWorkDescriptionFactory>().Create();
            var validWorkOrder = GetEntityFactory<WorkOrder>().Create(new { WorkDescription = mainBreakWorkDescription, ApprovedOn = DateTime.Now });
            var entity0 = GetFactory<MainBreakFactory>().Create(new { WorkOrder = validWorkOrder, FootageReplaced = 5 });
            var entity1 = GetFactory<MainBreakFactory>().Create(new { WorkOrder = validWorkOrder, FootageReplaced = 6 });

            var search = new SearchMainBreakPowerPlant();

            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true, 2))
            {
                helper.AreEqual(entity0.FootageReplaced, "FootageInstalled");
                helper.AreEqual(entity1.FootageReplaced, "FootageInstalled", 1);
                helper.AreEqual(entity0.WorkOrder.Id.ToString(), "WorkOrder");
                helper.AreEqual(entity1.WorkOrder.Id.ToString(), "WorkOrder", 1);
            }
        }

        #endregion
    }
}