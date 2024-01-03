using MapCallScheduler.JobHelpers.SapMaterial;
using MapCallScheduler.Tests.Library.JobHelpers.FileImports;
using MapCallScheduler.Tests.Library.JobHelpers.Sap;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallScheduler.Tests.JobHelpers.SapMaterial
{
    [TestClass]
    public class SapMaterialFileServiceTest : FileDownloadServiceTestBase<ISapMaterialServiceConfiguration, SapMaterialFileService>
    {
    }
}