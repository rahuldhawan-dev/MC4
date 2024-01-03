using MapCallScheduler.JobHelpers.W1V;
using MapCallScheduler.JobHelpers.W1V.DownloadServices;
using MapCallScheduler.Tests.Library.JobHelpers.FileImports;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallScheduler.Tests.JobHelpers.W1V.ImportTasks
{
    [TestClass]
    public class CustomerMaterialDownloadServiceTest
        : FileDownloadServiceTestBase<IW1VFileImportServiceConfiguration, CustomerMaterialService> { }
}
