using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using MapCall.Common.Model.Entities;
using MapCall.Common.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class TrainingSessionTest
    {
        private IContainer _container;

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
        }

        [TestMethod]
        public void TestToCalendarItemSetsCorrectCalendarColor()
        {
            var trainingModule = new TrainingModule {Title = "A training module"};
            var trainingRecord = new TrainingRecord {TrainingModule = trainingModule, MaximumClassSize = 3};
            var trainingSession = new TrainingSession
                {TrainingRecord = trainingRecord, StartDateTime = DateTime.Now, EndDateTime = DateTime.Now.AddHours(2)};

            var urlHelper = new UrlHelper(new ControllerContext().RequestContext);
            var target = trainingSession.ToCalendarItem(urlHelper);

            Assert.AreEqual(target.color, TrainingSession.CalendarColors.Background.LESS_THAN_FIFTY);

            trainingRecord.EmployeesScheduled.Add(new TrainingRecordScheduledEmployee());
            target = trainingSession.ToCalendarItem(urlHelper);

            Assert.AreEqual(target.color, TrainingSession.CalendarColors.Background.LESS_THAN_FIFTY);

            trainingRecord.EmployeesScheduled.Add(new TrainingRecordScheduledEmployee());
            target = trainingSession.ToCalendarItem(urlHelper);

            Assert.AreEqual(target.color, TrainingSession.CalendarColors.Background.LESS_THAN_ONE_HUNDRED);

            trainingRecord.EmployeesScheduled.Add(new TrainingRecordScheduledEmployee());
            target = trainingSession.ToCalendarItem(urlHelper);

            Assert.AreEqual(target.color, TrainingSession.CalendarColors.Background.ONE_HUNDRED);
        }

        [TestMethod]
        public void TestToCalendarItemSetsCorrectColorForPastItem()
        {
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            _container.Inject(dateTimeProvider.Object);
            dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(DateTime.Now);

            var trainingModule = new TrainingModule {Title = "A training module"};
            var trainingRecord = new TrainingRecord {TrainingModule = trainingModule, MaximumClassSize = 3};
            _container.BuildUp(trainingRecord);
            var trainingSession = new TrainingSession {
                TrainingRecord = trainingRecord, StartDateTime = DateTime.Now.AddDays(-1),
                EndDateTime = DateTime.Now.AddDays(-1).AddHours(2)
            };
            trainingRecord.TrainingSessions.Add(trainingSession);

            var urlHelper = new UrlHelper(new ControllerContext().RequestContext);
            var target = trainingSession.ToCalendarItem(urlHelper);

            Assert.AreEqual(TrainingSession.CalendarColors.Background.PAST, target.color);
        }

        [TestMethod]
        public void TestToCalendarItemSetsCorrectColorForCanceledItem()
        {
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            _container.Inject(dateTimeProvider.Object);
            dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(DateTime.Now);

            var trainingModule = new TrainingModule {Title = "A training module"};
            var trainingRecord = new TrainingRecord
                {TrainingModule = trainingModule, MaximumClassSize = 3, Canceled = true};
            var trainingSession = new TrainingSession {
                TrainingRecord = trainingRecord, StartDateTime = DateTime.Now.AddDays(-1),
                EndDateTime = DateTime.Now.AddDays(-1).AddHours(2)
            };
            trainingRecord.TrainingSessions.Add(trainingSession);

            var urlHelper = new UrlHelper(new ControllerContext().RequestContext);
            var target = trainingSession.ToCalendarItem(urlHelper);

            Assert.AreEqual(TrainingSession.CalendarColors.Background.CANCELED, target.color);
            Assert.AreEqual(TrainingSession.CalendarColors.Text.CANCELED, target.textColor);
        }
    }
}
