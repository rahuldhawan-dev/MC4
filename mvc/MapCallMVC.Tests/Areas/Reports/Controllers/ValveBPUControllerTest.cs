using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.Areas.Reports.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Reports.Controllers
{
    [TestClass]
    public class ValveBPUControllerTest : MapCallMvcControllerTestBase<ValveBPUController, Valve, ValveRepository>
    {
        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            GetFactory<ActiveAssetStatusFactory>().Create();
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/Reports/ValveBPU/Index");
                a.RequiresLoggedInUserOnly("~/Reports/ValveBPU/Search");
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            var factory = GetEntityFactory<Valve>();
            var billing1 = GetFactory<PublicValveBillingFactory>().Create();;
            var billing2 = GetFactory<MunicipalValveBillingFactory>().Create();
            var activeStatus = GetFactory<ActiveAssetStatusFactory>().Create();
            var retiredStatus = GetFactory<RetiredAssetStatusFactory>().Create();
            var smallSize = GetEntityFactory<ValveSize>().Create(new { Size = 2.0m });
            var largeSize = GetEntityFactory<ValveSize>().Create(new { Size = 12.0m });
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var val1 = factory.Create(new { ValveBilling = billing1, ValveSize = smallSize, Status = activeStatus, OperatingCenter = opc1 });
            var val2 = factory.Create(new { ValveBilling = billing1, ValveSize = largeSize, Status = activeStatus, OperatingCenter = opc1 });

            var search = new SearchValveBPUReport {OperatingCenter = opc1.Id};

            var result = _target.Index(search);

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(2, search.Count);
        }
        
        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var factory = GetEntityFactory<Valve>();
            var billing1 = GetFactory<PublicValveBillingFactory>().Create();;
            var billing2 = GetFactory<MunicipalValveBillingFactory>().Create();
            var activeStatus = GetFactory<ActiveAssetStatusFactory>().Create();
            var retiredStatus = GetFactory<RetiredAssetStatusFactory>().Create();
            var smallSize = GetEntityFactory<ValveSize>().Create(new { Size = 2.0m });
            var largeSize = GetEntityFactory<ValveSize>().Create(new { Size = 12.0m });
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var val1 = factory.Create(new { ValveBilling = billing1, ValveSize = smallSize, Status = activeStatus, OperatingCenter = opc1 });
            var val2 = factory.Create(new { ValveBilling = billing1, ValveSize = largeSize, Status = activeStatus, OperatingCenter = opc1 });

            var search = new SearchValveBPUReport();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(val1.ValveBilling, "ValveBilling");
                helper.AreEqual(val2.ValveBilling, "ValveBilling", 1);
                helper.AreEqual(Valve.Display.SIZE_RANGE_SMALL_VALVE, "SizeRange");
                helper.AreEqual(Valve.Display.SIZE_RANGE_LARGE_VALVE, "SizeRange", 1);
            }
            
        }
    }
}