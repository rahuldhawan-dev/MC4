using System;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.Areas.Reports.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;
using ReportModel = MapCallMVC.Areas.Reports.Models;

namespace MapCallMVC.Tests.Areas.Reports.Controllers
{
    [TestClass]
    public class NpdesRegulatorInspectionControllerTest : MapCallMvcControllerTestBase<NpdesRegulatorInspectionController, NpdesRegulatorInspection, NpdesRegulatorInspectionRepository>
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
                a.RequiresLoggedInUserOnly("~/Reports/NpdesRegulatorInspection/Index");
                a.RequiresLoggedInUserOnly("~/Reports/NpdesRegulatorInspection/Search");
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            var opc = GetFactory<OperatingCenterFactory>().Create();
            var inspection = GetFactory<NpdesRegulatorInspectionFactory>().Create(new {
                SewerOpening = GetFactory<SewerOpeningFactory>().Create(new {
                    LocationDescription = "description of location",
                    OutfallNumber = "007",
                    BodyOfWater = typeof(BodyOfWaterFactory)
                }),
                ArrivalDateTime = DateTime.Today,
                DepartureDateTime = DateTime.Today.AddHours(3)
            });
            var search = new ReportModel.SearchNpdesRegulatorInspection { OperatingCenter = opc.Id };
            
            var result = _target.Index(search);

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(1, search.Count);
        }
        
        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var inspection = GetFactory<NpdesRegulatorInspectionFactory>().Create(new {
                SewerOpening = GetFactory<SewerOpeningFactory>().Create(new {
                    LocationDescription = "description of location",
                    OutfallNumber = "007",
                    BodyOfWater = typeof(BodyOfWaterFactory)
                }),
                ArrivalDateTime = DateTime.Today,
                DepartureDateTime = DateTime.Today.AddHours(3)
            });

            var search = new ReportModel.SearchNpdesRegulatorInspection();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(inspection.BlockCondition, "Block Condition");
                helper.AreEqual(inspection.BlockCondition, "Block Condition", 1);
                helper.AreEqual(NpdesRegulatorInspectionReportItem.DisplayNames.LOCATION_DESCRIPTION, "Location Description");
                helper.AreEqual(NpdesRegulatorInspectionReportItem.DisplayNames.LOCATION_DESCRIPTION, "Location Description", 1);
            }
            
        }
    }
}