using MapCallScheduler.JobHelpers.SampleSiteDeactivation;
using MapCallScheduler.Jobs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallScheduler.Tests.Jobs
{
    [TestClass]
    public class SampleSiteDeactivateOnNewServiceRecordJobTest :
        MapCallJobWithProcessableServiceJobTestBase<SampleSiteDeactivateOnNewServiceRecordJob, ISampleSiteDeactivationProcessorService> { }
}
