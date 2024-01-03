using System;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class MeterChangeOutControllerTest : MapCallMvcControllerTestBase<MeterChangeOutController, MeterChangeOut>
    {
        private Mock<IDateTimeProvider> _dateTimeProvider;

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Use((_dateTimeProvider = new Mock<IDateTimeProvider>()).Object);
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeUpdateViewModel = (vm) => {
                // This status is needed for validation purposes in the update tests.
                GetEntityFactory<MeterChangeOutStatus>().Create(new { Description = "Some non-excluded description" });
                var model = (EditMeterChangeOut)vm;
                model.OutRead = "000000";
                model.StartRead = "000000";
            };
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var module = RoleModules.FieldServicesMeterChangeOuts;
            Authorization.Assert(a => {
                a.RequiresRole("~/FieldOperations/MeterChangeOut/Show/", module, RoleActions.Read);
                a.RequiresRole("~/FieldOperations/MeterChangeOut/Index/", module, RoleActions.Read);
                a.RequiresRole("~/FieldOperations/MeterChangeOut/Search/", module, RoleActions.Read);
                a.RequiresRole("~/FieldOperations/MeterChangeOut/ValidateNewSerialNumber/", module, RoleActions.Read);
                a.RequiresRole("~/FieldOperations/MeterChangeOut/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/MeterChangeOut/Update/", module, RoleActions.Edit);
            });
        }

        #region Show

        [TestMethod]
        public void TestShowRespondsToPdf()
        {
            var entity = GetFactory<MeterChangeOutFactory>().Create();
            InitializeControllerAndRequest("~/FieldOperations/MeterChangeOut/Show/" + entity.Id + ".pdf");
            var result = _target.Show(entity.Id);
            Assert.IsInstanceOfType(result, typeof(PdfResult));
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexRespondsToPdfAndDisablesPagingOnSearchForResults()
        {
            var entity = GetFactory<MeterChangeOutFactory>().Create();
            InitializeControllerAndRequest("~/FieldOperations/MeterChangeOut/Index.pdf");
            var search = new SearchMeterChangeOut() { EnablePaging = true };
            var result = _target.Index(search);
            Assert.IsInstanceOfType(result, typeof(PdfResult));
            Assert.IsFalse(search.EnablePaging);
        }

        [TestMethod]
        public void TestIndexExcelMarksClickAdvantextUpdatedWhenExportedAndMarkAdvantexExportTrue()
        {
            var search = new SearchMeterChangeOut {MarkAdvantexExport = true};
            var eq1 = GetEntityFactory<MeterChangeOut>().Create(new { ClickAdvantexUpdated = true });
            var eq2 = GetEntityFactory<MeterChangeOut>().Create(new { ClickAdvantexUpdated = false });
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(DateTime.Now);

            _target.Index(search);

            eq1 = Repository.Find(eq1.Id);
            Assert.IsTrue(eq1.ClickAdvantexUpdated);
            eq2 = Repository.Find(eq2.Id);
            Assert.IsTrue(eq2.ClickAdvantexUpdated);
        }

        [TestMethod]
        public void TestIndexExcelMarksClickAdvantextUpdatedFalseWhenExportedAndMarkAdvantexExportFalse()
        {
            var search = new SearchMeterChangeOut { MarkAdvantexExport = false };
            var eq1 = GetEntityFactory<MeterChangeOut>().Create(new { ClickAdvantexUpdated = false });
            var eq2 = GetEntityFactory<MeterChangeOut>().Create(new { ClickAdvantexUpdated = true });
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(DateTime.Now);

            var result = _target.Index(search) as ExcelResult;

            eq1 = Repository.Find(eq1.Id);
            Assert.IsFalse(eq1.ClickAdvantexUpdated);
            eq2 = Repository.Find(eq2.Id);
            Assert.IsFalse(eq2.ClickAdvantexUpdated);
        }

        [TestMethod]
        public void TestIndexExcelWhenExportedDoesNotMarkAdvantexExport()
        {
            var search = new SearchMeterChangeOut();
            var eq1 = GetEntityFactory<MeterChangeOut>().Create(new { ClickAdvantexUpdated = true });
            var eq2 = GetEntityFactory<MeterChangeOut>().Create(new { ClickAdvantexUpdated = false });
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(DateTime.Now);

            var result = _target.Index(search) as ExcelResult;

            eq1 = Repository.Find(eq1.Id);
            Assert.IsTrue(eq1.ClickAdvantexUpdated);
            eq2 = Repository.Find(eq2.Id);
            Assert.IsFalse(eq2.ClickAdvantexUpdated);
        }

        [TestMethod]
        public void TestIndexExcelGetsCorrectFileName()
        {
            var search = new SearchMeterChangeOut();
            var eq1 = GetEntityFactory<MeterChangeOut>().Create(new { ClickAdvantexUpdated = true });
            var eq2 = GetEntityFactory<MeterChangeOut>().Create(new { ClickAdvantexUpdated = false });
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;
            var date = new DateTime(2016,11,4,1,30,0);
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(date);
            _container.Inject(_dateTimeProvider.Object);

            var result = _target.Index(search) as ExcelResult;

            Assert.AreEqual($"{date.Year}-{date.Month}-{date.Day}-{date:tt}.xlsx", result.FileDownloadName);
        }

        [TestMethod]
        public void TestIndexExcelGetsCorrectFileNameBetween12And1PM()
        {
            var search = new SearchMeterChangeOut();
            var eq1 = GetEntityFactory<MeterChangeOut>().Create(new { ClickAdvantexUpdated = true });
            var eq2 = GetEntityFactory<MeterChangeOut>().Create(new { ClickAdvantexUpdated = false });
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;
            var date = new DateTime(2016, 11, 4, 12, 30, 0);
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(date);
            _container.Inject(_dateTimeProvider.Object);

            var result = _target.Index(search) as ExcelResult;

            Assert.AreEqual($"{date.Year}-{date.Month}-{date.Day}-{date.AddHours(-1):tt}.xlsx", result.FileDownloadName);
        }

        [TestMethod]
        public void TestIndexExcelGetsCorrectFileNameAfter1PM()
        {
            var search = new SearchMeterChangeOut();
            var eq1 = GetEntityFactory<MeterChangeOut>().Create(new { ClickAdvantexUpdated = true });
            var eq2 = GetEntityFactory<MeterChangeOut>().Create(new { ClickAdvantexUpdated = false });
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;
            var date = new DateTime(2016, 11, 4, 14, 30, 0);
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(date);
            _container.Inject(_dateTimeProvider.Object);

            var result = _target.Index(search) as ExcelResult;

            Assert.AreEqual($"{date.Year}-{date.Month}-{date.Day}-{date:tt}.xlsx", result.FileDownloadName);
        }

        [TestMethod]
        public void TestIndexExcelGetsCorrectFileNameForSearchWithCrew()
        {
            var crew = GetEntityFactory<ContractorMeterCrew>().Create(new { Description = "TwoLive" });
            var search = new SearchMeterChangeOut { CalledInByContractorMeterCrew = crew.Id};
            var eq1 = GetEntityFactory<MeterChangeOut>().Create(new { ClickAdvantexUpdated = true });
            var eq2 = GetEntityFactory<MeterChangeOut>().Create(new { ClickAdvantexUpdated = false });
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;
            var date = new DateTime(2016, 11, 4, 8, 30, 0);
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(date);
            _container.Inject(_dateTimeProvider.Object);

            var result = _target.Index(search) as ExcelResult;

            Assert.AreEqual($"{date.Year}-{date.Month}-{date.Day}-TwoLive-{date:tt}.xlsx", result.FileDownloadName);
        }

        #endregion
    }
}