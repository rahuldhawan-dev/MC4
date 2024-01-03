using MapCallScheduler.JobHelpers.AssetUploadProcessor;
using MapCallScheduler.Jobs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallScheduler.Tests.Jobs
{
    [TestClass]
    public class AssetUploadProcessorJobTest : MapCallJobWithProcessableServiceJobTestBase<AssetUploadProcessorJob, IAssetUploadProcessorService> {}
}