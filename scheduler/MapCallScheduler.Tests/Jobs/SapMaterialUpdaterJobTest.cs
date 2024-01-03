using System;
using MapCallScheduler.JobHelpers.SapMaterial;
using MapCallScheduler.Jobs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallScheduler.Tests.Jobs
{
    [TestClass]
    public class SapMaterialUpdaterJobTest :
        MapCallJobWithProcessableServiceJobTestBase<SapMaterialUpdaterJob, ISapMaterialService>
    {
        [TestMethod]
        public void TestExecuteEmailsExtraAddressOnError()
        {
            var e = new InvalidOperationException("blah");

            _service.Setup(x => x.Process()).Throws(e);

            _target.Execute(null);

            _emailer.Verify(
                x =>
                    x.SendMessage(SapMaterialUpdaterJob.AMWATER_EMAIL, SapMaterialUpdaterJob.ERROR_MESSAGE,
                        $"An exception of type {e.GetType()} was encountered with the following message: {e.Message}",
                        false));
        }
    }
}
