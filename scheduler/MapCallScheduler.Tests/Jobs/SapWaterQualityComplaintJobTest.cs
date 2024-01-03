using MapCallScheduler.JobHelpers.SapWaterQualityComplaint;
using MapCallScheduler.Jobs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallScheduler.Tests.Jobs
{
    [TestClass]
    public class SapWaterQualityComplaintJobTest:
        MapCallJobWithProcessableServiceJobTestBase<SapWaterQualityComplaintJob, ISapWaterQualityComplaintService> { }
}
