using log4net;
using MapCallScheduler.Library.Filesystem;
using MapCallScheduler.Library.JobHelpers.Common;

namespace MapCallScheduler.JobHelpers.W1V.DownloadServices
{
    public class CustomerMaterialService : FileDownloadServiceBase<IW1VFileImportServiceConfiguration>
    {
        #region Constants

        public const string FILE_TYPE = "ServiceLineExtract",
                            FILE_PATTERN = FILE_TYPE + "_*.csv";

        #endregion

        #region Properties

        public override string FileType => FILE_TYPE;
        public override string FilePattern => FILE_PATTERN;

        #endregion

        #region Constructors

        public CustomerMaterialService(
            IW1VFileImportServiceConfiguration config,
            IFileClientFactory fileClientFactory,
            ILog log)
            : base(
                config,
                fileClientFactory,
                log) { }

        #endregion
    }
}
