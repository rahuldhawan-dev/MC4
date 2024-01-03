using System;
using MapCallScheduler.JobHelpers.SystemDeliveryEntry.DumpTasks;
using MapCallScheduler.Jobs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using Moq;

namespace MapCallScheduler.Tests.Jobs
{
    [TestClass]
    public class MonthlySystemDeliveryEntryFileDumpTest : MapCallJobWithProcessableServiceJobTestBase<MonthlySystemDeliveryEntryFileDumpJob, IMonthlySystemDeliveryEntryFileDumpService>
    {
        #region Private Members

        private DateTime _now;
        private TestDateTimeProvider _dateTimeProvider;

        #endregion

        protected override void InitializeBeforeTarget()
        {
            _container.Inject<IDateTimeProvider>(_dateTimeProvider = new TestDateTimeProvider(_now = new DateTime(2021, 03, 04)));
        }

        [TestMethod]
        public void TestServiceDoesRunOnFourthBusinessDayOfMonthOfNonQuarterEndMonth()
        {
            _target.Execute(null);

            _service.Verify(s => s.Process(), Times.Once);
        }

        [TestMethod]
        public void TestServiceRunsOnSecondBusinessDayofQuarterEndMonth()
        {
            _now = new DateTime(2021, 3, 2);

            _dateTimeProvider.SetNow(_now);

            _target.Execute(null);

            _service.Verify(s => s.Process(), Times.Never);

            _service.Invocations.Clear();

            _now = new DateTime(2021, 4, 2);

            _dateTimeProvider.SetNow(_now);

            _target.Execute(null);

            _service.Verify(s => s.Process(), Times.Once);

            _service.Invocations.Clear();
        }
    }
}
