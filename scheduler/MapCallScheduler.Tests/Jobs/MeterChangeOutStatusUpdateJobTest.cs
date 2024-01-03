using System;
using MapCallScheduler.JobHelpers.MeterChangeOutStatusUpdate;
using MapCallScheduler.Jobs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallScheduler.Tests.Jobs
{
    [TestClass]
    public class MeterChangeOutStatusUpdateJobTest : MapCallJobWithProcessableServiceJobTestBase<MeterChangeOutStatusUpdateJob, IMeterChangeOutStatusUpdateService>
    {
        [TestMethod]
        public void TestExecuteEmailsExtraAddressOnError()
        {
            var e = new InvalidOperationException("blah");

            _service.Setup(x => x.Process()).Throws(e);

            _target.Execute(null);

            _emailer.Verify(
                x =>
                    x.SendMessage(MeterChangeOutStatusUpdateJob.AMWATER_EMAIL, "Error updating Meter Change Out status",
                        $"An exception of type {e.GetType()} was encountered with the following message: {e.Message}",
                        false));
        }
    }
}
