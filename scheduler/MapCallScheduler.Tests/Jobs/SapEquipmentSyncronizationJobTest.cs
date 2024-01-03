using System;
using MapCallScheduler.JobHelpers.SAPDataSyncronization;
using MapCallScheduler.Jobs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallScheduler.Tests.Jobs
{
    [TestClass]
    public class SapEquipmentSyncronizationJobTest : MapCallJobWithProcessableServiceJobTestBase<SAPEquipmentSyncronizationJob, ISAPSyncronizationService>
    {
        [TestMethod]
        public void TestExecuteEmailsExtraAddressOnError()
        {
            var e = new InvalidOperationException("flergh");

            _service.Setup(x => x.Process()).Throws(e);

            _target.Execute(null);

            _emailer.Verify(x => x.SendMessage(SAPEquipmentSyncronizationJob.AMWATER_EMAIL, SAPEquipmentSyncronizationJob.ERROR_MESSAGE,
                        $"An exception of type {e.GetType()} was encountered with the following message: {e.Message}",
                        false));
        }
    }
}