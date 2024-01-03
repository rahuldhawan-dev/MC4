using MapCallScheduler.JobHelpers.LeakAlert;
using MapCallScheduler.Tests.Library.JobHelpers.FileImports;
using MapCallScheduler.Tests.Library.JobHelpers.Sap;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallScheduler.Tests.JobHelpers.LeakAlert
{
    [TestClass]
    public class LeakAlertFileServiceTest : FileDownloadServiceTestBase<ILeakAlertServiceConfiguration, LeakAlertFileService>
    {
        
    }
}
