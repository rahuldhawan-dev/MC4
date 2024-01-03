using MapCallScheduler.JobHelpers.GISMessageBroker;
using MapCallScheduler.Jobs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallScheduler.Tests.Jobs
{
    [TestClass]
    public class GISMessageBrokerIntegrationJobTest : MapCallJobWithProcessableServiceJobTestBase<GISMessageBrokerIntegrationJob, IGISMessageBrokerService> {}
}