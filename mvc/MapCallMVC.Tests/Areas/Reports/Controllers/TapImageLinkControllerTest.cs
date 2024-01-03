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
using MMSINC.Utilities.Pdf;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Reports.Controllers
{
    [TestClass]
    public class TapImageLinkControllerTest : MapCallMvcControllerTestBase<TapImageLinkController, TapImage, TapImageRepository>
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
                a.RequiresLoggedInUserOnly("~/Reports/TapImageLink/Index");
                a.RequiresLoggedInUserOnly("~/Reports/TapImageLink/Search");
            });
        }

        #region Index

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var tapImage1 = GetFactory<TapImageFactory>().Create(new { OperatingCenter = operatingCenter, PremiseNumber = "1234512345" });
            var tapImage2 = GetFactory<TapImageFactory>().Create(new { OperatingCenter = operatingCenter, PremiseNumber = "1234512346" });
            var tapImage3 = GetFactory<TapImageFactory>().Create(new { OperatingCenter = operatingCenter, PremiseNumber = "123451234" });

            var search = new SearchTapImageLink();

            var result = _target.Index(search) as ViewResult;
            var resultModel = ((SearchTapImageLink)result.Model).Results.ToList();

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(2, resultModel.Count);
            Assert.AreEqual(tapImage1.PremiseNumber, resultModel[0].PremiseNumber);
            Assert.AreEqual(tapImage2.PremiseNumber, resultModel[1].PremiseNumber);
        }

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var tapImage1 = GetFactory<TapImageFactory>().Create(new { OperatingCenter = operatingCenter, PremiseNumber = "1234512345" });
            var tapImage2 = GetFactory<TapImageFactory>().Create(new { OperatingCenter = operatingCenter, PremiseNumber = "1234512346" });
            var tapImage3 = GetFactory<TapImageFactory>().Create(new { OperatingCenter = operatingCenter, PremiseNumber = "123451234" });
            var search = new SearchTapImageLink();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(tapImage1.PremiseNumber, "Premise Number");
                helper.AreEqual(tapImage2.PremiseNumber, "Premise Number", 1);
            }
        }

        #endregion
    }
}