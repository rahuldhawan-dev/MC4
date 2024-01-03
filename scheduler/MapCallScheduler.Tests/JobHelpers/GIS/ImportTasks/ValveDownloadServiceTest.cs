using MapCallScheduler.JobHelpers.GIS;
using MapCallScheduler.JobHelpers.GIS.DownloadServices;
using MapCallScheduler.Tests.Library.JobHelpers.FileImports;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallScheduler.Tests.JobHelpers.GIS.ImportTasks
{
    [TestClass]
    public class ValveDownloadServiceTest : FileDownloadServiceTestBase<IGISFileImportServiceConfiguration, ValveService> {}
}
