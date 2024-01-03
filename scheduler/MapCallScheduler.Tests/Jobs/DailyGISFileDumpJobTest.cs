using MapCallScheduler.JobHelpers.GIS;
using MapCallScheduler.Jobs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallScheduler.Tests.Jobs
{
    [TestClass]
    public class DailyGISFileDumpJobTest : MapCallJobWithProcessableServiceJobTestBase<DailyGISFileDumpJob, IDailyGISFileDumpService> {}
}