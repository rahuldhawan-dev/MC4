using MapCallScheduler.JobHelpers.W1V;
using MapCallScheduler.Jobs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallScheduler.Tests.Jobs
{
    [TestClass]
    public class DailyW1VFileImportJobTest
        : MapCallJobWithProcessableServiceJobTestBase<DailyW1VFileImportJob, IDailyW1VFileImportService> {}
}
