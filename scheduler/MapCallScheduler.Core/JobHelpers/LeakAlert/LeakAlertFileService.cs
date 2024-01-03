using log4net;
using MapCallScheduler.Library.Filesystem;
using MapCallScheduler.Library.JobHelpers.Sap;

namespace MapCallScheduler.JobHelpers.LeakAlert
{
    public class LeakAlertFileService : SapFileServiceBase<ILeakAlertServiceConfiguration>, ILeakAlertFileService
    {
        #region Constants

        public const string FILE_TYPE = "NJAWPCNExport";

        #endregion

        #region Properties

        public override string FileType => FILE_TYPE;

        public override string FilePattern => $"{FILE_TYPE}*";

        #endregion

        #region Constructors

        public LeakAlertFileService(ILeakAlertServiceConfiguration config, IFileClientFactory fileClientFactory, ILog log) : base(config, fileClientFactory, log) { }

        #endregion
    }
}
