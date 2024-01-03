using System;
using MapCallScheduler.JobHelpers.LeakAlert;
using MapCallScheduler.Jobs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallScheduler.Tests.Jobs
{
    [TestClass]
    public class LeakAlertUpdaterJobTest : MapCallJobWithProcessableServiceJobTestBase<LeakAlertUpdaterJob, ILeakAlertService>
    {
        [TestMethod]
        public void TestExecuteEmailsExtraAddressOnError()
        {
            var e = new InvalidOperationException("blah");

            _service.Setup(x => x.Process()).Throws(e);

            _target.Execute(null);

            _emailer.Verify(
                x =>
                    x.SendMessage(LeakAlertUpdaterJob.AMWATER_EMAIL, LeakAlertUpdaterJob.ERROR_MESSAGE,
                        $"An exception of type {e.GetType()} was encountered with the following message: {e.Message}",
                        false));
        }
    }
}
