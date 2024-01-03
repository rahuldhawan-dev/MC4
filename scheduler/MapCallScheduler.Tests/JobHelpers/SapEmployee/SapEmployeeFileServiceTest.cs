using MapCallScheduler.JobHelpers.SapEmployee;
using MapCallScheduler.Tests.Library.JobHelpers.FileImports;
using MapCallScheduler.Tests.Library.JobHelpers.Sap;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallScheduler.Tests.JobHelpers.SapEmployee
{
    [TestClass]
    public class SapEmployeeFileServiceTest : FileDownloadServiceTestBase<ISapEmployeeServiceConfiguration, SapEmployeeFileService>
    {
    }
}