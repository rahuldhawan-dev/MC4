using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Reports.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Controllers
{
    [TestClass]
    public class MainBreaksAndServiceLineRepairsByMonthControllerTest : MapCallMvcControllerTestBase<MainBreaksAndServiceLineRepairsByMonthController, WorkOrder>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IWorkOrderRepository>().Use<WorkOrderRepository>();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.IndexDisplaysViewWhenNoResults = true;
        }

        #endregion

        private void SetupData()
        {
            var waterReplace = GetFactory<WaterMainBreakReplaceWorkDescriptionFactory>().Create();
            var waterRepair = GetFactory<WaterMainBreakRepairWorkDescriptionFactory>().Create();
            var sewerReplace = GetFactory<SewerMainBreakReplaceWorkDescriptionFactory>().Create();
            var sewerRepair = GetFactory<SewerMainBreakRepairWorkDescriptionFactory>().Create();
            var serviceRepair = GetFactory<ServiceLineRepairWorkDescriptionFactory>().Create();
            var other = GetFactory<WaterMainBleedersWorkDescriptionFactory>().Create();
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create();

            //valid
            var wo1 = GetEntityFactory<WorkOrder>().Create(new { WorkDescription = waterReplace, DateCompleted = new DateTime(2014, 1, 1), OperatingCenter = operatingCenter });
            var wo11 = GetEntityFactory<WorkOrder>().Create(new { WorkDescription = waterReplace, DateCompleted = new DateTime(2014, 2, 1), OperatingCenter = operatingCenter });
            var wo2 = GetEntityFactory<WorkOrder>().Create(new { WorkDescription = waterRepair, DateCompleted = new DateTime(2014, 1, 1), OperatingCenter = operatingCenter });
            var wo21 = GetEntityFactory<WorkOrder>().Create(new { WorkDescription = waterRepair, DateCompleted = new DateTime(2014, 2, 1), OperatingCenter = operatingCenter });
            var wo3 = GetEntityFactory<WorkOrder>().Create(new { WorkDescription = sewerReplace, DateCompleted = new DateTime(2014, 1, 1), OperatingCenter = operatingCenter });
            var wo31 = GetEntityFactory<WorkOrder>().Create(new { WorkDescription = sewerReplace, DateCompleted = new DateTime(2014, 2, 1), OperatingCenter = operatingCenter });
            var wo4 = GetEntityFactory<WorkOrder>().Create(new { WorkDescription = sewerRepair, DateCompleted = new DateTime(2014, 1, 1), OperatingCenter = operatingCenter });
            var wo41 = GetEntityFactory<WorkOrder>().Create(new { WorkDescription = sewerRepair, DateCompleted = new DateTime(2014, 2, 1), OperatingCenter = operatingCenter });
            var wo5 = GetEntityFactory<WorkOrder>().Create(new { WorkDescription = serviceRepair, DateCompleted = new DateTime(2014, 1, 1), OperatingCenter = operatingCenter });
            var wo51 = GetEntityFactory<WorkOrder>().Create(new { WorkDescription = serviceRepair, DateCompleted = new DateTime(2014, 2, 1), OperatingCenter = operatingCenter });
            var wo52 = GetEntityFactory<WorkOrder>().Create(new { WorkDescription = serviceRepair, DateCompleted = new DateTime(2014, 2, 1), OperatingCenter = operatingCenter });
            //invalid
            var wo6 = GetEntityFactory<WorkOrder>().Create(new { WorkDescription = other, DateCompleted = Lambdas.GetNow });
            var wo7 = GetEntityFactory<WorkOrder>().Create(new { WorkDescription = waterReplace });
            
            GetFactory<RoleFactory>().Create(RoleModules.FieldServicesWorkManagement, operatingCenter, _currentUser, RoleActions.Read);
        }

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var module = RoleModules.FieldServicesWorkManagement;
                a.RequiresRole("~/Reports/MainBreaksAndServiceLineRepairsByMonth/Search", module);
                a.RequiresRole("~/Reports/MainBreaksAndServiceLineRepairsByMonth/Index", module);
            });
        }

        #region Index

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            var search = new SearchMainBreaksAndServiceLineRepairsByMonth();

            var result = _target.Index(search) as ViewResult;
            var resultModel = (IEnumerable<MainBreaksAndServiceLineRepairsReportViewModel>)result.Model;

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(0, resultModel.Count());

            SetupData();
            result = _target.Index(search) as ViewResult;
            resultModel = (IEnumerable<MainBreaksAndServiceLineRepairsReportViewModel>)result.Model;

            Assert.AreEqual(5, resultModel.Count());
        }

        [TestMethod]
        public override void TestIndexRedirectsToSearchIfModelStateIsInvalid()
        {
            Assert.Inconclusive("Implement and test me");
        }

        #endregion
    }
}
