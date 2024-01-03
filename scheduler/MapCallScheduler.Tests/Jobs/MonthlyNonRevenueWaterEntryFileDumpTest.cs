using System;
using MapCallScheduler.JobHelpers.NonRevenueWater;
using MapCallScheduler.Jobs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using Moq;

namespace MapCallScheduler.Tests.Jobs
{
    [TestClass]
    public class MonthlyNonRevenueWaterEntryFileDumpTest : MapCallJobWithProcessableServiceJobTestBase<MonthlyNonRevenueWaterEntryFileDumpJob, IMonthlyNonRevenueWaterEntryFileDumpService>
    {
        private TestDateTimeProvider _dateTimeProvider;
        private DateTime _now;

        protected override void InitializeBeforeTarget()
        {
            _container.Inject<IDateTimeProvider>(_dateTimeProvider = new TestDateTimeProvider(_now = new DateTime(2023, 06, 06)));
        }

        [TestMethod]
        public void Test_DoProcess_ExecutesOnFourthBusinessDayWhenCurrentMonthIsNotQuarterEndMonth()
        {
            _target.Execute(null);
            
            _service.Verify(x => x.Process(), Times.Once);
        }

        [TestMethod]
        public void Test_DoProcess_ExecutesOnSecondBusinessDayWhenCurrentMonthIsQuarterEndMonth()
        {
            _now = new DateTime(2023, 07, 05);
            _dateTimeProvider.SetNow(_now);

            _target.Execute(null);
            
            _service.Verify(x => x.Process(), Times.Once);
        }

        [TestMethod]
        public void Test_DoProcess_DoesNotExecuteOnInvalidDays()
        {
            _now = new DateTime(2023, 07, 04);
            _dateTimeProvider.SetNow(_now);
            
            _target.Execute(null);
            
            _service.Verify(x => x.Process(), Times.Never);
        }
    }
}
