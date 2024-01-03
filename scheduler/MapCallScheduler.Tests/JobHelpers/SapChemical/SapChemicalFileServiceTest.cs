using MapCallScheduler.JobHelpers.SapChemical;
using MapCallScheduler.Tests.Library.JobHelpers.FileImports;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallScheduler.Tests.JobHelpers.SapChemical
{
    [TestClass]
    public class SapChemicalFileServiceTest : FileDownloadServiceTestBase<ISapChemicalServiceConfiguration, SapChemicalFileService>
    {
    }
}
