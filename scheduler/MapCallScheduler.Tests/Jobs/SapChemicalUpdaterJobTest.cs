using System;
using MapCallScheduler.JobHelpers.SapChemical;
using MapCallScheduler.Jobs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallScheduler.Tests.Jobs
{
    [TestClass]
    public class SapChemicalUpdaterJobTest :
        MapCallJobWithProcessableServiceJobTestBase<SapChemicalUpdaterJob, ISapChemicalService>
    {
        [TestMethod]
        public void TestExecuteEmailsExtraAddressOnError()
        {
            var e = new InvalidOperationException("blah");

            _service.Setup(x => x.Process()).Throws(e);

            _target.Execute(null);

            _emailer.Verify(
                x =>
                    x.SendMessage(SapChemicalUpdaterJob.AMWATER_EMAIL, SapChemicalUpdaterJob.ERROR_MESSAGE,
                        $"An exception of type {e.GetType()} was encountered with the following message: {e.Message}",
                        false));
        }
    }
}
