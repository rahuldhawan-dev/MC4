using MapCallScheduler.JobHelpers.NSIPremiseFileLink;
using MapCallScheduler.JobHelpers.W1V;
using MapCallScheduler.Jobs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallScheduler.Tests.Jobs
{
    [TestClass]
    public class NSIPremiseFileLinkJobTest
        : MapCallJobWithProcessableServiceJobTestBase<NSIPremiseFileLinkJob, INSIPremiseFileLinkService> {}
}
