using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Reports.Controllers;
using MapCallMVC.Areas.Reports.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace MapCallMVC.Tests.Areas.Reports.Controllers
{
    [TestClass]
    public class RestorationAccrualReportControllerTest : MapCallMvcControllerTestBase<RestorationAccrualReportController, Restoration, RestorationRepository>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = RoleModules.FieldServicesWorkManagement;
            Authorization.Assert(a => {
                a.RequiresRole("~/Reports/RestorationAccrualReport/Search", role);
                a.RequiresRole("~/Reports/RestorationAccrualReport/Index", role);
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            var capitalAccountingType = GetFactory<CapitalAccountingTypeFactory>().Create();
            var workDescription = GetFactory<CheckNoWaterWorkDescriptionFactory>().Create(new {
                AccountingType = capitalAccountingType
            });
            var workOrder = GetFactory<CompletedWorkOrderFactory>().Create(new {
                WorkDescription = workDescription,
                ApprovedOn = DateTime.Now
            });
            var restoration = GetEntityFactory<Restoration>().Create(new {
                OperatingCenter = workOrder.OperatingCenter,
                WorkOrder = workOrder,
                FinalRestorationDate = (DateTime?)null 
            });
            
            // override needed due to required fields on search model.
            var model = new SearchRestorationAccrualReport();
            model.OperatingCenter = restoration.OperatingCenter.Id;
            model.WorkOrderDateCompleted = new MMSINC.Data.RequiredDateRange {
                Start = workOrder.DateCompleted.Value.AddDays(-100),
                End = workOrder.DateCompleted.Value.AddDays(100)
            };
            model.AccountingType = capitalAccountingType.Id;

            _target.Index(model);

            Assert.AreEqual(1, model.Results.Count());
        }
    }
}
