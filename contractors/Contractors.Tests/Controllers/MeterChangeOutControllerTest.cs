using System;
using System.Web.Mvc;
using Contractors.Controllers;
using Contractors.Data.Models.Repositories;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace Contractors.Tests.Controllers
{
    [TestClass]
    public class MeterChangeOutControllerTest : ContractorControllerTestBase<MeterChangeOutController, MeterChangeOut>
    {
        private Mock<IDateTimeProvider> _dateTimeProvider;

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _dateTimeProvider = e.For<IDateTimeProvider>().Mock();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = () => {
                var mco = GetEntityFactory<MeterChangeOut>().Create();
                mco.Contract.Contractor = _currentUser.Contractor;
                Session.Save(mco.Contract);
                Session.Flush();
                return mco;
            }; 
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
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/MeterChangeOut/Show/");
                a.RequiresLoggedInUserOnly("~/MeterChangeOut/Index/");
                a.RequiresLoggedInUserOnly("~/MeterChangeOut/Search/");
                a.RequiresLoggedInUserOnly("~/MeterChangeOut/ValidateNewSerialNumber/");
                a.RequiresLoggedInUserOnly("~/MeterChangeOut/Edit/");
                a.RequiresLoggedInUserOnly("~/MeterChangeOut/Update/");
            });
        }

        #region Show

        [TestMethod]
        public void TestShowRespondsToPdf()
        {
            var entity = GetFactory<MeterChangeOutFactory>().Create();
            entity.Contract.Contractor = _currentUser.Contractor;
            Session.Flush();
            InitializeControllerAndRequest("~/MeterChangeOut/Show/" + entity.Id + ".pdf");
            var result = _target.Show(entity.Id);
            Assert.IsInstanceOfType(result, typeof(PdfResult));
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexRespondsToPdfAndDisablesPagingOnSearchForResults()
        {
            var entity = GetFactory<MeterChangeOutFactory>().Create();
            entity.Contract.Contractor = _currentUser.Contractor;
            Session.Flush();
            InitializeControllerAndRequest("~/FieldOperations/MeterChangeOut/Index.pdf");
            var search = new SearchMeterChangeOut() { EnablePaging = true };
            var result = _target.Index(search);
            Assert.IsInstanceOfType(result, typeof(PdfResult));
            Assert.IsFalse(search.EnablePaging);
        }

        [TestMethod]
        public void TestIndexExcelMarksClickAdvantextUpdatedWhenExportedAndMarkAdvantexExportTrue()
        {
            var search = new SearchMeterChangeOut { MarkAdvantexExport = true };
            var eq1 = GetEntityFactory<MeterChangeOut>().Create(new { ClickAdvantexUpdated = true });
            var eq2 = GetEntityFactory<MeterChangeOut>().Create(new { ClickAdvantexUpdated = false });
            eq1.Contract.Contractor = _currentUser.Contractor;
            eq2.Contract.Contractor = _currentUser.Contractor;
            _dateTimeProvider.Setup(x => x.GetCurrentDate())
                             .Returns(DateTime.Now);
            Session.Flush();
           
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

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
            eq1.Contract.Contractor = _currentUser.Contractor;
            eq2.Contract.Contractor = _currentUser.Contractor;
            Session.Flush();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;
            _dateTimeProvider.Setup(x => x.GetCurrentDate())
                             .Returns(DateTime.Now);

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
            _dateTimeProvider.Setup(x => x.GetCurrentDate())
                             .Returns(DateTime.Now);

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
            var date = new DateTime(2016, 11, 4, 1, 30, 0);
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(date);

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

            var result = _target.Index(search) as ExcelResult;

            Assert.AreEqual($"{date.Year}-{date.Month}-{date.Day}-{date:tt}.xlsx", result.FileDownloadName);
        }

        [TestMethod]
        public void TestIndexExcelGetsCorrectFileNameForSearchWithCrew()
        {
            _container.Inject<IContractorMeterCrewRepository>(
                _container.GetInstance<ContractorMeterCrewRepository>());

            var crew = GetEntityFactory<ContractorMeterCrew>().Create(new { Description = "TwoLive", Contractor = _currentUser.Contractor });
            var search = new SearchMeterChangeOut { CalledInByContractorMeterCrew = crew.Id };
            var eq1 = GetEntityFactory<MeterChangeOut>().Create(new { ClickAdvantexUpdated = true });
            var eq2 = GetEntityFactory<MeterChangeOut>().Create(new { ClickAdvantexUpdated = false });
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;
            var date = new DateTime(2016, 11, 4, 8, 30, 0);
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(date);

            var result = _target.Index(search) as ExcelResult;

            Assert.AreEqual($"{date.Year}-{date.Month}-{date.Day}-TwoLive-{date:tt}.xlsx", result.FileDownloadName);
        }

        #endregion
    }
}