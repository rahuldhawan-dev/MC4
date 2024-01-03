using System;
using System.Linq;
using System.Runtime.ExceptionServices;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCall.Common.Testing.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using Moq;
using NHibernate;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class AbsenceNotificationRepositoryTest : MapCallEmployeeSecuredRepositoryTestBase<AbsenceNotification,
        AbsenceNotificationRepository>
    {
        #region Private Members

        private Mock<IDateTimeProvider> _dateTimeProvider;
        private DateTime _now;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Use((_dateTimeProvider = new Mock<IDateTimeProvider>()).Object);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _now = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(_now);
        }

        #endregion

        #region GetEmployeesWithAbsencesLessThanAYearOld / Occurrence Report

        [TestMethod]
        public void
            TestGetAbsenceNotificationsLessThanAYearOldGetsAbsenceNotificationsLessThanAYearOldWithNonFmlaAbsences()
        {
            var employees = GetFactory<ActiveEmployeeFactory>().CreateList(4);
            var progressiveDiscipline = GetEntityFactory<ProgressiveDiscipline>().Create();
            var absenceNotification1 = GetEntityFactory<AbsenceNotification>().Create(new {
                Employee = employees[0],
                StartDate = _now.AddMonths(-6),
                EndDate = _now.AddMonths(-6).AddDays(1),
                TotalHoursOfAbsence = 10m,
                ProgressiveDiscipline = progressiveDiscipline
            });
            var absenceNotification2 = GetEntityFactory<AbsenceNotification>()
               .Create(new {Employee = employees[0], StartDate = _now.SubtractYears(2)});
            var absenceNotification3 = GetEntityFactory<AbsenceNotification>()
               .Create(new {Employee = employees[0], StartDate = _now.SubtractYears(1)});
            var absenceNotification4 = GetEntityFactory<AbsenceNotification>()
               .Create(new {Employee = employees[2], StartDate = _now.SubtractYears(5)});
            var absenceNotification5 = GetEntityFactory<AbsenceNotification>()
               .Create(new {Employee = employees[2], StartDate = _now.SubtractYears(1)});
            var fmlaCase = GetEntityFactory<FamilyMedicalLeaveActCase>().Create();
            var absenceNotification6 = GetEntityFactory<AbsenceNotification>().Create(new
                {Employee = employees[2], StartDate = _now.AddMonths(-2), FamilyMedicalLeaveActCase = fmlaCase});

            var search = new TestSearchOccurrence();

            var results = Repository.GetNonFMLAAbsencesLessThanAYearOld(search);

            Assert.AreEqual(3, results.Count());
            var first = results.First();
            MyAssert.AreClose(absenceNotification1.StartDate.Value, first.StartDate.Value);
            MyAssert.AreClose(absenceNotification1.EndDate.Value, first.EndDate.Value);
            Assert.AreEqual(absenceNotification1.TotalHoursOfAbsence.Value, first.TotalHoursOfAbsence);
            Assert.AreSame(progressiveDiscipline, first.ProgressiveDiscipline);
        }

        #endregion
    }

    public class TestSearchOccurrence : SearchSet<OccurrenceReportItem>, ISearchOccurrence
    {
        public int? OperatingCenter { get; set; }
    }
}
