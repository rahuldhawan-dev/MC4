using System;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Operations.Models.ViewModels;
using MapCallMVC.Areas.Reports.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Utilities;
using Moq;

namespace MapCallMVC.Tests.Areas.Reports.Controllers
{
    [TestClass]
    public class OccurrenceControllerTest : MapCallMvcControllerTestBase<OccurrenceController, AbsenceNotification, AbsenceNotificationRepository>
    {
        #region Private Members

        private Mock<IDateTimeProvider> _dateTimeProvider;
        private DateTime _now;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _now = DateTime.Now;
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(_now);

            _container.Inject(_dateTimeProvider.Object);
        }
        
        #endregion
        
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = RoleModules.OperationsManagement;
            Authorization.Assert(a => {
                a.RequiresRole("~/Reports/Occurrence/Search", role);
                a.RequiresRole("~/Reports/Occurrence/Index", role);
            });
        }

        #region Index

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            var employees = GetFactory<ActiveEmployeeFactory>().CreateList(4);
            var progressiveDiscipline = GetEntityFactory<ProgressiveDiscipline>().Create();
            var absenceNotification1 = GetEntityFactory<AbsenceNotification>().Create(new
            {
                Employee = employees[0],
                StartDate = _now.AddMonths(-6),
                EndDate = _now.AddMonths(-6).AddDays(1),
                TotalHoursOfAbsence = 10m,
                ProgressiveDiscipline = progressiveDiscipline
            });
            var absenceNotification2 = GetEntityFactory<AbsenceNotification>().Create(new
            {
                Employee = employees[0],
                StartDate = _now.AddMonths(-4),
                EndDate = _now.AddMonths(-4).AddDays(1),
                TotalHoursOfAbsence = 8m
            });

            var search = new SearchOccurrence();
            _target.ControllerContext = new ControllerContext();

            var result = _target.Index(search) as ViewResult;
            var resultModel = ((SearchOccurrence)result.Model).Results.ToList();

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(2, resultModel.Count);
            Assert.AreEqual(absenceNotification1.TotalHoursOfAbsence, resultModel[0].TotalHoursOfAbsence);
            Assert.AreEqual(absenceNotification2.TotalHoursOfAbsence, resultModel[1].TotalHoursOfAbsence);
        }

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var employees = GetFactory<ActiveEmployeeFactory>().CreateList(4);
            var progressiveDiscipline = GetEntityFactory<ProgressiveDiscipline>().Create();
            var absenceNotification1 = GetEntityFactory<AbsenceNotification>().Create(new
            {
                Employee = employees[0],
                StartDate = _now.AddMonths(-6),
                EndDate = _now.AddMonths(-6).AddDays(1),
                TotalHoursOfAbsence = 10m,
                ProgressiveDiscipline = progressiveDiscipline
            });
            var absenceNotification2 = GetEntityFactory<AbsenceNotification>().Create(new
            {
                Employee = employees[0],
                StartDate = _now.AddMonths(-4),
                EndDate = _now.AddMonths(-4).AddDays(1),
                TotalHoursOfAbsence = 8m
            });
            var search = new SearchOccurrence();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(absenceNotification1.TotalHoursOfAbsence, "TotalHoursOfAbsence");
                helper.AreEqual(absenceNotification2.TotalHoursOfAbsence, "TotalHoursOfAbsence", 1);
            }
        }

        #endregion
    }
}