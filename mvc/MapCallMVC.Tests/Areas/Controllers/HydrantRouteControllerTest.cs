using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.Areas.Reports.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Controllers
{
    [TestClass]
    public class HydrantRouteControllerTest : MapCallMvcControllerTestBase<HydrantRouteController, Hydrant, HydrantRepository>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/Reports/HydrantRoute/Index");
                a.RequiresLoggedInUserOnly("~/Reports/HydrantRoute/Search");
            });
        }

        #region Index

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var town = GetEntityFactory<Town>().Create();
            var hydrantsRoute1 = GetEntityFactory<Hydrant>().CreateList(3, new { Route = 1, OperatingCenter = operatingCenter, Town = town });
            var hydrantsRoute2 = GetEntityFactory<Hydrant>().CreateList(5, new { Route = 2, OperatingCenter = operatingCenter, Town = town });

            var search = new SearchHydrantRouteReport();
            _target.ControllerContext = new ControllerContext();

            var result = _target.Index(search) as ViewResult;
            var resultModel = ((SearchHydrantRouteReport)result.Model).Results.ToList();

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(2, resultModel.Count);
            Assert.AreEqual(hydrantsRoute1[0].Route, resultModel[0].Route);
            Assert.AreEqual(hydrantsRoute2[0].Route, resultModel[1].Route);
        }

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var town = GetEntityFactory<Town>().Create();
            var hydrantsRoute1 = GetEntityFactory<Hydrant>().CreateList(3, new { Route = 1, OperatingCenter = operatingCenter, Town = town });
            var hydrantsRoute2 = GetEntityFactory<Hydrant>().CreateList(5, new { Route = 2, OperatingCenter = operatingCenter, Town = town });

            var search = new SearchHydrantRouteReport();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(hydrantsRoute1[0].Route, "Route");
                helper.AreEqual(hydrantsRoute2[0].Route, "Route", 1);
                helper.AreEqual(hydrantsRoute1[0].OperatingCenter, "OperatingCenter");
                helper.AreEqual(hydrantsRoute2[0].OperatingCenter, "OperatingCenter", 1);
            }
        }

        #endregion
    }
}