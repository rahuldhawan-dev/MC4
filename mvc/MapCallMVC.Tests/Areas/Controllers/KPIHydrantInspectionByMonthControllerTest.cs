using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.Areas.Reports.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Controllers
{
    [TestClass]
    public class KPIHydrantInspectionByMonthControllerTest : MapCallMvcControllerTestBase<KPIHydrantInspectionByMonthController, HydrantInspection, HydrantInspectionRepository>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.IndexDisplaysViewWhenNoResults = true;
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/Reports/KPIHydrantInspectionByMonth/Search");
                a.RequiresLoggedInUserOnly("~/Reports/KPIHydrantInspectionByMonth/Index");
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            var now = Lambdas.GetNow();
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>()
               .Create(new {ZoneStartYear = DateTime.Today.Year});
            var activeHydrantStatus = GetFactory<ActiveAssetStatusFactory>().Create();
            var municipalBillingStatus = GetFactory<MunicipalHydrantBillingFactory>().Create();
            var publicBillingStatus = GetFactory<PublicHydrantBillingFactory>().Create();
            var hyd1 = GetEntityFactory<Hydrant>().Create(new { SAPEquipmentId = 666,Status = activeHydrantStatus, HydrantBilling = publicBillingStatus, OperatingCenter = operatingCenter, Zone = 1 });
            var hyd2 = GetEntityFactory<Hydrant>().Create(new { SAPEquipmentId = 667,Status = activeHydrantStatus, HydrantBilling = publicBillingStatus, OperatingCenter = operatingCenter, Zone = 1 });
            var hit1 = GetEntityFactory<HydrantInspectionType>().Create();
            var hit2 = GetEntityFactory<HydrantInspectionType>().Create();
            var hydInsp1 = GetEntityFactory<HydrantInspection>().Create(new { Hydrant = hyd1, DateInspected = now, HydrantInspectionType = hit1 });
            var hydInsp2 = GetEntityFactory<HydrantInspection>().Create(new { Hydrant = hyd2, DateInspected = now, HydrantInspectionType = hit2 });
            var search = new SearchKPIHydrantInspectionsByMonth { Year = now.Year, OperatingCenter = new[]{operatingCenter.Id}};

            var result = _target.Index(search) as ViewResult;
            var resultModel = ((IEnumerable<KPIHydrantsInspectedReport>)result.Model).ToList(); 

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(4, resultModel.Count());
            Assert.AreEqual(hit1.Description, resultModel[0].HydrantInspectionType);
            Assert.AreEqual(hit2.Description, resultModel[1].HydrantInspectionType);
            Assert.AreEqual("Total", resultModel[2].HydrantInspectionType);
            Assert.AreEqual("Total", resultModel[3].OperatingCenter);
        }

        [TestMethod]
        public override void TestIndexRedirectsToSearchIfModelStateIsInvalid()
        {
            Assert.Inconclusive("Implement and test me");
        }

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var now = Lambdas.GetNow();
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>()
               .Create(new {ZoneStartYear = DateTime.Today.Year});
            var activeHydrantStatus = GetFactory<ActiveAssetStatusFactory>().Create();
            var municipalBillingStatus = GetFactory<MunicipalHydrantBillingFactory>().Create();
            var publicBillingStatus = GetFactory<PublicHydrantBillingFactory>().Create();
            var hyd1 = GetEntityFactory<Hydrant>().Create(new { SAPEquipmentId = 666,Status = activeHydrantStatus, HydrantBilling = publicBillingStatus, OperatingCenter = operatingCenter, Zone = 1 });
            var hyd2 = GetEntityFactory<Hydrant>().Create(new { SAPEquipmentId = 667,Status = activeHydrantStatus, HydrantBilling = publicBillingStatus, OperatingCenter = operatingCenter, Zone = 1 });
            var hit1 = GetEntityFactory<HydrantInspectionType>().Create();
            var hit2 = GetEntityFactory<HydrantInspectionType>().Create();
            var hydInsp1 = GetEntityFactory<HydrantInspection>().Create(new { Hydrant = hyd1, DateInspected = now, HydrantInspectionType = hit1 });
            var hydInsp2 = GetEntityFactory<HydrantInspection>().Create(new { Hydrant = hyd2, DateInspected = now, HydrantInspectionType = hit2 });
            var search = new SearchKPIHydrantInspectionsByMonth { Year = now.Year, OperatingCenter = new[]{operatingCenter.Id}};
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] = ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(hit1.Description, "HydrantInspectionType");
                helper.AreEqual(hit2.Description, "HydrantInspectionType", 1);
            }
        }
    }
}