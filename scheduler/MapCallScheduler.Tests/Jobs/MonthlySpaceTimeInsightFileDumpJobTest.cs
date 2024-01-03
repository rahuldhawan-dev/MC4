using System;
using MapCallScheduler.JobHelpers.SpaceTimeInsight;
using MapCallScheduler.Jobs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.Jobs
{
    [TestClass]
    public class MonthlySpaceTimeInsightFileDumpJobTest : MapCallJobWithProcessableServiceJobTestBase<MonthlySpaceTimeInsightFileDumpJob, ISpaceTimeInsightMonthlyFileDumpService>
    {
        #region Private Members

        private DateTime _now;
        private TestDateTimeProvider _dateTimeProvider;

        #endregion

        protected override void InitializeBeforeTarget()
        {
            _container.Inject<IDateTimeProvider>(
                _dateTimeProvider = new TestDateTimeProvider(_now = new DateTime(1984, 02, 01)));
        }

        [TestMethod]
        public void TestServiceDoesNotRunUnlessFirstOfMonth()
        {
            _now = new DateTime(1984, 1, 1);

            for (var current = _now.Date; current.Year == _now.Year; current = current.AddDays(1))
            {
                _dateTimeProvider.SetNow(current);

                _target.Execute(null);

                if (current == current.GetBeginningOfMonth())
                {
                    _service.Verify(s => s.Process());
                }
                else
                {
                    _service.Verify(s => s.Process(), Times.Never);
                }

                _service.ResetCalls();
            }
        }
    }
}