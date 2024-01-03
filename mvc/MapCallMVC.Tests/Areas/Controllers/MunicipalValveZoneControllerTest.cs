using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.Areas.Reports.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Controllers
{
    [TestClass]
    public class MunicipalValveZoneControllerTest : MapCallMvcControllerTestBase<MunicipalValveZoneController, MunicipalValveZone, MunicipalValveZoneRepository>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                a.RequiresLoggedInUserOnly("~/Reports/MunicipalValveZone/Index");
                a.RequiresLoggedInUserOnly("~/Reports/MunicipalValveZone/Search");
            });
        }

        #region Index/Search

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            var factory = GetEntityFactory<Valve>();
            var billing1 = GetFactory<PublicValveBillingFactory>().Create();
            var billing2 = GetFactory<MunicipalValveBillingFactory>().Create();
            var activeStatus = GetFactory<ActiveAssetStatusFactory>().Create();
            var retiredStatus = GetFactory<RetiredAssetStatusFactory>().Create();
            var smallSize = GetEntityFactory<ValveSize>().Create(new { Size = 2.0m });
            var largeSize = GetEntityFactory<ValveSize>().Create(new { Size = 12.0m });
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCenters.Add(opc1);
            Session.Clear();
            var valveZone1 = GetEntityFactory<ValveZone>().Create(new { Description = "1" });
            var valveZone2 = GetEntityFactory<ValveZone>().Create(new { Description = "2" });
            var val1 = factory.Create(new { ValveBilling = billing1, ValveSize = smallSize, Status = activeStatus, OperatingCenter = opc1, Town = town, ValveZone = valveZone1 });
            var val2 = factory.Create(new { ValveBilling = billing1, ValveSize = smallSize, Status = activeStatus, OperatingCenter = opc1, Town = town, ValveZone = valveZone1 });
            var val3 = factory.Create(new { ValveBilling = billing1, ValveSize = smallSize, Status = activeStatus, OperatingCenter = opc1, Town = town, ValveZone = valveZone1 });
            var muniValZone1 = GetEntityFactory<MunicipalValveZone>().Create(new { OperatingCenter = opc1, Town = town, SmallValveZone = valveZone1, LargeValveZone = valveZone2 });
            var search = new SearchMunicipalValveZoneReport { OperatingCenter = opc1.Id };

            var result = _target.Index(search);

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(1, search.Count);
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
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCenters.Add(opc1);
            Session.Clear();
            var valveZone1 = GetEntityFactory<ValveZone>().Create(new { Description = "1" });
            var valveZone2 = GetEntityFactory<ValveZone>().Create(new { Description = "2" });

            var val1 = factory.Create(new { ValveBilling = billing1, ValveSize = smallSize, Status = activeStatus, OperatingCenter = opc1, Town = town, ValveZone = valveZone1 });
            var val2 = factory.Create(new { ValveBilling = billing1, ValveSize = smallSize, Status = activeStatus, OperatingCenter = opc1, Town = town, ValveZone = valveZone1 });
            var val3 = factory.Create(new { ValveBilling = billing1, ValveSize = smallSize, Status = activeStatus, OperatingCenter = opc1, Town = town, ValveZone = valveZone1 });


            var muniValZone1 = GetEntityFactory<MunicipalValveZone>().Create(new { OperatingCenter = opc1, Town = town, SmallValveZone = valveZone1, LargeValveZone = valveZone2 });
            
            var search = new SearchMunicipalValveZoneReport();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(opc1.OperatingCenterCode, "OperatingCenter");
                helper.AreEqual(town.ShortName, "Town");
                helper.AreEqual(3, "SmallValves");
                helper.AreEqual(0, "LargeValves");
            }
        }
     
        #endregion
    }
}