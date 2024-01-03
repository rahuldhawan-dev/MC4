using log4net;
using MapCallScheduler.Library.Filesystem;
using MapCallScheduler.Library.JobHelpers.Common;

namespace MapCallScheduler.JobHelpers.GIS.DownloadServices
{
    public class ValveService : FileDownloadServiceBase<IGISFileImportServiceConfiguration>
    {
        #region Constants

        public const string FILE_TYPE = "GIS_Valves",
                            FILE_PATTERN = FILE_TYPE + "_*.json";

        #endregion

        #region Properties

        public override string FileType => FILE_TYPE;
        public override string FilePattern => FILE_PATTERN;

        #endregion

        #region Constructors

        public ValveService(IGISFileImportServiceConfiguration config, IFileClientFactory fileClientFactory, ILog log) : base(config, fileClientFactory, log) { }

        #endregion
    }
}
