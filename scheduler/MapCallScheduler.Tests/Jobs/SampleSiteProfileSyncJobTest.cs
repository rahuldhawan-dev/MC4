using MapCallScheduler.JobHelpers.LIMSSynchronization;
using MapCallScheduler.Jobs.LIMSSynchronization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallScheduler.Tests.Jobs
{
    [TestClass]
    public class SampleSiteProfileSyncJobTest :
        MapCallJobWithProcessableServiceJobTestBase<SampleSiteProfileSyncJob, ISampleSiteProfileSyncService> { }
}
