using log4net;
using MapCallScheduler.Library.Filesystem;
using MapCallScheduler.Library.JobHelpers.Common;

namespace MapCallScheduler.JobHelpers.GIS.DownloadServices
{
    public class ServiceService : FileDownloadServiceBase<IGISFileImportServiceConfiguration>
    {
        #region Constants

        public const string FILE_TYPE = "GIS_Services",
                            FILE_PATTERN = FILE_TYPE + "_*.json";

        #endregion

        #region Properties

        public override string FileType => FILE_TYPE;
        public override string FilePattern => FILE_PATTERN;

        #endregion

        #region Constructors

        public ServiceService(IGISFileImportServiceConfiguration config, IFileClientFactory fileClientFactory, ILog log) : base(config, fileClientFactory, log) { }

        #endregion
    }
}
