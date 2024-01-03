using MapCallScheduler.JobHelpers.LeakAlert;
using MapCallScheduler.Tests.Library.JobHelpers.Sap;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallScheduler.Tests.JobHelpers.LeakAlert
{
    [TestClass]
    public class LeakAlertServiceTest : SapFileProcessingServiceTestBase<LeakAlertService, ILeakAlertFileService, ILeakAlertUpdaterService>
    {
        
    }
}
