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
    public class ValvesOperatedByMonthControllerTest : MapCallMvcControllerTestBase<ValvesOperatedByMonthController, ValveInspection, ValveInspectionRepository>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.IndexDisplaysViewWhenNoResults = true;
        }

        protected override MapCall.Common.Model.Entities.Users.User CreateUser()
        {
            var user = base.CreateUser();
            user.IsAdmin = true;
            return user;
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                a.RequiresLoggedInUserOnly("~/Reports/ValvesOperatedByMonth/Search");
                a.RequiresLoggedInUserOnly("~/Reports/ValvesOperatedByMonth/Index");
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            var year = 2011;
            var date = new DateTime(year, 1, 1);
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var valveZones = GetEntityFactory<ValveZone>().CreateList(7);
            var activeStatus = GetFactory<ActiveAssetStatusFactory>().Create();
            var publicBilling = GetFactory<PublicValveBillingFactory>().Create();
            var valveSizeSmall = GetEntityFactory<ValveSize>().Create(new { Size = 2.0m });
            var valveSizeLarge = GetEntityFactory<ValveSize>().Create(new { Size = 12.0m });
            // the good
            var valve1 = GetEntityFactory<Valve>().Create(new { OperatingCenter = operatingCenter, ValveSize = valveSizeSmall, DateInstalled = date, Status = activeStatus, ValveBilling = publicBilling, ValveZone = valveZones[0] });
            var valve2 = GetEntityFactory<Valve>().Create(new { OperatingCenter = operatingCenter, ValveSize = valveSizeSmall, DateInstalled = date, Status = activeStatus, ValveBilling = publicBilling, ValveZone = valveZones[0] });
            var valve3 = GetEntityFactory<Valve>().Create(new { OperatingCenter = operatingCenter, ValveSize = valveSizeLarge, DateInstalled = date, Status = activeStatus, ValveBilling = publicBilling, ValveZone = valveZones[0] });
            var valve4 = GetEntityFactory<Valve>().Create(new { OperatingCenter = operatingCenter, ValveSize = valveSizeSmall, DateInstalled = date, Status = activeStatus, ValveBilling = publicBilling, ValveZone = valveZones[0] });
            var valve5 = GetEntityFactory<Valve>().Create(new { OperatingCenter = operatingCenter, DateInstalled = date, Status = activeStatus, ValveBilling = publicBilling, ValveZone = valveZones[0] });
            var vi1 = GetEntityFactory<ValveInspection>().Create(new { Valve = valve1, Inspected = true, DateInspected = date.AddDays(1) });
            var vi2 = GetEntityFactory<ValveInspection>().Create(new { Valve = valve2, Inspected = true, DateInspected = date.AddMonths(1).AddDays(1) });
            var vi3 = GetEntityFactory<ValveInspection>().Create(new { Valve = valve3, Inspected = true, DateInspected = date.AddMonths(2).AddDays(1) });
            var vi4 = GetEntityFactory<ValveInspection>().Create(new { Valve = valve4, Inspected = true, DateInspected = date.AddMonths(2).AddDays(1) });
            var vi5 = GetEntityFactory<ValveInspection>().Create(new { Valve = valve5, Inspected = true, DateInspected = date.AddMonths(2).AddDays(1) });

            var search = new SearchValvesOperatedByMonth { Year = year, OperatingCenter = new[] { operatingCenter.Id } };

            var result = _target.Index(search) as ViewResult;
            var resultModel = ((IEnumerable<ValvesOperatedByMonthReport>)result.Model).ToList();

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(7, resultModel.Count());
            Assert.AreEqual(Valve.Display.SIZE_RANGE_SMALL_VALVE, resultModel[0].SizeRange);
            Assert.AreEqual(Valve.Display.SIZE_RANGE_SMALL_VALVE, resultModel[1].SizeRange);
            Assert.AreEqual(Valve.Display.SIZE_RANGE_LARGE_VALVE, resultModel[2].SizeRange);
            Assert.AreEqual(Valve.Display.SIZE_RANGE_LARGE_VALVE, resultModel[3].SizeRange);
            Assert.AreEqual(Valve.Display.SIZE_RANGE_NULL, resultModel[4].SizeRange);
            Assert.AreEqual(Valve.Display.SIZE_RANGE_NULL, resultModel[5].SizeRange);
            Assert.AreEqual("Total", resultModel[6].SizeRange);
        }

        [TestMethod]
        public void TestIndexXMLSExportsExcel()
        {
            var year = 2011;
            var date = new DateTime(year, 1, 1);
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var valveZones = GetEntityFactory<ValveZone>().CreateList(7);
            var activeStatus = GetFactory<ActiveAssetStatusFactory>().Create();
            var publicBilling = GetFactory<PublicValveBillingFactory>().Create();
            var valveSizeSmall = GetEntityFactory<ValveSize>().Create(new { Size = 2.0m });
            var valveSizeLarge = GetEntityFactory<ValveSize>().Create(new { Size = 12.0m });
            // the good
            var valve1 = GetEntityFactory<Valve>().Create(new { OperatingCenter = operatingCenter, ValveSize = valveSizeSmall, DateInstalled = date, Status = activeStatus, ValveBilling = publicBilling, ValveZone = valveZones[0] });
            var valve2 = GetEntityFactory<Valve>().Create(new { OperatingCenter = operatingCenter, ValveSize = valveSizeSmall, DateInstalled = date, Status = activeStatus, ValveBilling = publicBilling, ValveZone = valveZones[0] });
            var valve3 = GetEntityFactory<Valve>().Create(new { OperatingCenter = operatingCenter, ValveSize = valveSizeLarge, DateInstalled = date, Status = activeStatus, ValveBilling = publicBilling, ValveZone = valveZones[0] });
            var valve4 = GetEntityFactory<Valve>().Create(new { OperatingCenter = operatingCenter, ValveSize = valveSizeSmall, DateInstalled = date, Status = activeStatus, ValveBilling = publicBilling, ValveZone = valveZones[0] });
            var valve5 = GetEntityFactory<Valve>().Create(new { OperatingCenter = operatingCenter, DateInstalled = date, Status = activeStatus, ValveBilling = publicBilling, ValveZone = valveZones[0] });
            var vi1 = GetEntityFactory<ValveInspection>().Create(new { Valve = valve1, Inspected = true, DateInspected = date.AddDays(1) });
            var vi2 = GetEntityFactory<ValveInspection>().Create(new { Valve = valve2, Inspected = true, DateInspected = date.AddMonths(1).AddDays(1) });
            var vi3 = GetEntityFactory<ValveInspection>().Create(new { Valve = valve3, Inspected = true, DateInspected = date.AddMonths(2).AddDays(1) });
            var vi4 = GetEntityFactory<ValveInspection>().Create(new { Valve = valve4, Inspected = true, DateInspected = date.AddMonths(2).AddDays(1) });
            var vi5 = GetEntityFactory<ValveInspection>().Create(new { Valve = valve5, Inspected = true, DateInspected = date.AddMonths(2).AddDays(1) });

            var search = new SearchValvesOperatedByMonth{ Year = year, OperatingCenter = new[] { operatingCenter.Id } };

            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] = ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(Valve.Display.SIZE_RANGE_SMALL_VALVE, "SizeRange");
                helper.AreEqual(Valve.Display.SIZE_RANGE_SMALL_VALVE, "SizeRange", 1);
                helper.AreEqual(Valve.Display.SIZE_RANGE_LARGE_VALVE, "SizeRange", 2);
                helper.AreEqual(Valve.Display.SIZE_RANGE_LARGE_VALVE, "SizeRange", 3);
                helper.AreEqual(Valve.Display.SIZE_RANGE_NULL, "SizeRange", 4);
                helper.AreEqual(Valve.Display.SIZE_RANGE_NULL, "SizeRange", 5);
                helper.AreEqual("Total", "SizeRange", 6);
            }
        }

        [TestMethod]
        public override void TestIndexRedirectsToSearchIfModelStateIsInvalid()
        {
            Assert.Inconclusive("Properly implement validation and then remove this test.");
        }
    }
}