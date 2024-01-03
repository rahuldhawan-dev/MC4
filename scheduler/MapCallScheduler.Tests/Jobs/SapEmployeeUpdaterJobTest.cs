using System;
using MapCallScheduler.JobHelpers.SapEmployee;
using MapCallScheduler.Jobs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallScheduler.Tests.Jobs
{
    [TestClass]
    public class SapEmployeeUpdaterJobTest :
        MapCallJobWithProcessableServiceJobTestBase<SapEmployeeUpdaterJob, ISapEmployeeService>
    {
        [TestMethod]
        public void TestExecuteEmailsExtraAddressOnError()
        {
            var e = new InvalidOperationException("blah");

            _service.Setup(x => x.Process()).Throws(e);

            _target.Execute(null);

            _emailer.Verify(
                x =>
                    x.SendMessage(SapEmployeeUpdaterJob.AMWATER_EMAIL, "Error updating MapCall employee from SAP",
                        $"An exception of type {e.GetType()} was encountered with the following message: {e.Message}",
                        false));
        }
    }
}
