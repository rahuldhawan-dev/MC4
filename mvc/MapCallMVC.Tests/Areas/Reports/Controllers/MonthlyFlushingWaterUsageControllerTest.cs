using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.Areas.Reports.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Reports.Controllers
{
    [TestClass]
    public class MonthlyFlushingWaterUsageControllerTest : MapCallMvcControllerTestBase<MonthlyFlushingWaterUsageController, HydrantInspection, HydrantInspectionRepository>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.IndexDisplaysViewWhenNoResults = true;
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                const RoleModules role = RoleModules.FieldServicesAssets;
                a.RequiresRole("~/Reports/MonthlyFlushingWaterUsage/Index", role);
                a.RequiresRole("~/Reports/MonthlyFlushingWaterUsage/Search", role);
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var hydrant = GetFactory<HydrantFactory>().Create(new { OperatingCenter = opc1 });
            var inspection = GetFactory<HydrantInspectionFactory>().Create(new {
                Hydrant = hydrant,
                DateInspected = new DateTime(2015,1,1),
                MinutesFlowed = 10m,
                GPM = 10m
            });

            var search = new SearchHydrantFlushing { OperatingCenter = opc1.Id };
            var result = _target.Index(search);
            MvcAssert.IsViewNamed(result, "Index");

            Assert.AreEqual(12, search.Count);
        }
        
        #endregion
    }
}
