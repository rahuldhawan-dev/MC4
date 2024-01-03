using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.Areas.Reports.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Utilities.Pdf;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Controllers
{
    [TestClass]
    public class ValveImageLinkControllerTest : MapCallMvcControllerTestBase<ValveImageLinkController, ValveImage, ValveImageRepository>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IImageToPdfConverter>().Use(new Mock<IImageToPdfConverter>().Object);
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                a.RequiresLoggedInUserOnly("~/Reports/ValveImageLink/Index");
                a.RequiresLoggedInUserOnly("~/Reports/ValveImageLink/Search");
            });
        }

        #region Index

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var town = GetEntityFactory<Town>().Create();
            var val1 = GetEntityFactory<Valve>().Create(new { OperatingCenter = operatingCenter, Town = town, SAPEquipmentId = 321 });
            var valveImage1 = GetEntityFactory<ValveImage>().Create(new {Valve = val1});
            var val2 = GetEntityFactory<Valve>().Create(new { OperatingCenter = operatingCenter, Town = town, SAPEquipmentId = 123 });
            var valveImage2 = GetEntityFactory<ValveImage>().Create(new { Valve = val2 });

            var search = new SearchValveImageLink();
            _target.ControllerContext = new ControllerContext();

            var result = _target.Index(search) as ViewResult;
            var resultModel = ((SearchValveImageLink)result.Model).Results.ToList();

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(2, resultModel.Count);
            Assert.AreEqual(val1.SAPEquipmentId, resultModel[0].SAPEquipmentId);
            Assert.AreEqual(val2.SAPEquipmentId, resultModel[1].SAPEquipmentId);
        }

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var town = GetEntityFactory<Town>().Create();
            var val1 = GetEntityFactory<Valve>().Create(new { OperatingCenter = operatingCenter, Town = town, SAPEquipmentId = 321 });
            var valveImage1 = GetEntityFactory<ValveImage>().Create(new { Valve = val1 });
            var val2 = GetEntityFactory<Valve>().Create(new { OperatingCenter = operatingCenter, Town = town, SAPEquipmentId = 123 });
            var valveImage2 = GetEntityFactory<ValveImage>().Create(new { Valve = val2 });
            var search = new SearchValveImageLink();
            _target.ControllerContext = new ControllerContext();
                _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                    ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(val1.SAPEquipmentId, "SAP Equipment Number");
                helper.AreEqual(val2.SAPEquipmentId, "SAP Equipment Number", 1);
            }
        }

        #endregion
    }
}