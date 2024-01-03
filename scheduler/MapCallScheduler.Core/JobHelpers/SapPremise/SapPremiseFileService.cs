using log4net;
using MapCallScheduler.Library.Filesystem;
using MapCallScheduler.Library.JobHelpers.Sap;

namespace MapCallScheduler.JobHelpers.SapPremise
{
    public class SapPremiseFileService : SapFileServiceBase<ISapPremiseServiceConfiguration>, ISapPremiseFileService
    {
        #region Constants

        public const string FILE_TYPE = "Premise";

        #endregion

        #region Properties

        public override string FileType => FILE_TYPE;

        #endregion

        #region Constructors

        public SapPremiseFileService(
            ISapPremiseServiceConfiguration config,
            IFileClientFactory fileClientFactory, ILog log)
            : base(config, fileClientFactory, log) {}

        #endregion
    }
}
