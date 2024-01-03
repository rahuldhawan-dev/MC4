using log4net;
using MapCallScheduler.JobHelpers.AssetUploadProcessor;
using MapCallScheduler.Library.Email;
using MapCallScheduler.Metadata;
using Quartz;
using StructureMap;

namespace MapCallScheduler.Jobs
{
    [Immediate,Minutely(5), DisallowConcurrentExecution, NoConfigureSession]
    public class AssetUploadProcessorJob : MapCallJobWithProcessableServiceBase<IAssetUploadProcessorService>
    {
        public AssetUploadProcessorJob(ILog log, IAssetUploadProcessorService service, IDeveloperEmailer emailer) : base(log, service, emailer) { }
    }
}
