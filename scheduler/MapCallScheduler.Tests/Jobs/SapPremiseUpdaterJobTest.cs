using System;
using MapCallScheduler.JobHelpers.SapPremise;
using MapCallScheduler.Jobs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallScheduler.Tests.Jobs
{
    [TestClass]
    public class SapPremiseUpdaterJobTest :
        MapCallJobWithProcessableServiceJobTestBase<SapPremiseUpdaterJob, ISapPremiseService>
    {
        [TestMethod]
        public void TestExecuteEmailsExtraAddressOnError()
        {
            var e = new InvalidOperationException("blah");

            _service.Setup(x => x.Process()).Throws(e);

            _target.Execute(null);

            _emailer.Verify(
                x =>
                    x.SendMessage(SapPremiseUpdaterJob.AMWATER_EMAIL, SapPremiseUpdaterJob.ERROR_MESSAGE,
                        $"An exception of type {e.GetType()} was encountered with the following message: {e.Message}",
                        false));
        }
    }
}