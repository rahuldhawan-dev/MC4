using MapCallScheduler.JobHelpers.NonRevenueWaterEntryCreator;
using MapCallScheduler.Jobs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallScheduler.Tests.Jobs
{
    [TestClass]
    public class NonRevenueWaterEntryCreatorJobTest : MapCallJobWithProcessableServiceJobTestBase<
        NonRevenueWaterEntryCreatorJob, INonRevenueWaterEntryCreatorService> { }
}
