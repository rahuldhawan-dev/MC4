using log4net;
using MapCallScheduler.Library.Filesystem;
using MapCallScheduler.Library.JobHelpers.Common;

namespace MapCallScheduler.JobHelpers.W1V
{
    public class W1VFileDownloadService
        : FileDownloadServiceBase<W1VFileImportServiceConfiguration>, IW1VFileDownloadService
    {
        public override string FileType => null;
        public override string FilePattern => "ServiceLineExtract_*.csv";

        public W1VFileDownloadService(W1VFileImportServiceConfiguration config, IFileClientFactory fileClientFactory, ILog log) : base(config, fileClientFactory, log) { }
    }
    
    public interface IW1VFileDownloadService : IFileDownloadService { }
}
