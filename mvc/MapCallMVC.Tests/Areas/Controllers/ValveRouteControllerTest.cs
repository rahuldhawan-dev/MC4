using System.Linq;
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
    public class ValveRouteControllerTest : MapCallMvcControllerTestBase<ValveRouteController, Valve, ValveRepository>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/Reports/ValveRoute/Index");
                a.RequiresLoggedInUserOnly("~/Reports/ValveRoute/Search");
            });
        }

        #region Index

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var town = GetEntityFactory<Town>().Create();
            var activeStatus = GetFactory<ActiveAssetStatusFactory>().Create();
            var valvesRoute1 = GetEntityFactory<Valve>().CreateList(3, new { Route = 1, OperatingCenter = operatingCenter, Town = town, Status = activeStatus });
            var valvesRoute2 = GetEntityFactory<Valve>().CreateList(5, new { Route = 2, OperatingCenter = operatingCenter, Town = town, Status = activeStatus });

            var search = new SearchValveRouteReport();
            _target.ControllerContext = new ControllerContext();

            var result = _target.Index(search) as ViewResult;
            var resultModel = ((SearchValveRouteReport)result.Model).Results.ToList();

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(2, resultModel.Count);
            Assert.AreEqual(valvesRoute1[0].Route, resultModel[0].Route);
            Assert.AreEqual(valvesRoute2[0].Route, resultModel[1].Route);
        }

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var town = GetEntityFactory<Town>().Create();
            var activeStatus = GetFactory<ActiveAssetStatusFactory>().Create(); 
            var valvesRoute1 = GetEntityFactory<Valve>().CreateList(3, new { Route = 1, OperatingCenter = operatingCenter, Town = town, Status = activeStatus });
            var valvesRoute2 = GetEntityFactory<Valve>().CreateList(5, new { Route = 2, OperatingCenter = operatingCenter, Town = town, Status = activeStatus });

            var search = new SearchValveRouteReport();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(valvesRoute1[0].Route, "Route");
                helper.AreEqual(valvesRoute2[0].Route, "Route", 1);
                helper.AreEqual(valvesRoute1[0].OperatingCenter, "OperatingCenter");
                helper.AreEqual(valvesRoute2[0].OperatingCenter, "OperatingCenter", 1);
            }
        }

        #endregion
    }
}