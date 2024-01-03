using log4net;
using MapCallScheduler.Library.Filesystem;
using MapCallScheduler.Library.JobHelpers.Sap;

namespace MapCallScheduler.JobHelpers.SapEmployee
{
    public class SapEmployeeFileService : SapFileServiceBase<ISapEmployeeServiceConfiguration>, ISapEmployeeFileService
    {
        #region Constants

        public const string FILE_TYPE = "Employee";

        #endregion

        #region Properties

        public override string FileType => FILE_TYPE;

        #endregion

        #region Constructors

        public SapEmployeeFileService(ISapEmployeeServiceConfiguration config, IFileClientFactory fileClientFactory, ILog log) : base(config, fileClientFactory, log) {}

        #endregion
    }
}