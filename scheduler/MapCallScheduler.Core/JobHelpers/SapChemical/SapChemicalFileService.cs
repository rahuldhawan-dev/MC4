using log4net;
using MapCallScheduler.Library.Filesystem;
using MapCallScheduler.Library.JobHelpers.Sap;

namespace MapCallScheduler.JobHelpers.SapChemical
{
    public class SapChemicalFileService : SapFileServiceBase<ISapChemicalServiceConfiguration>, ISapChemicalFileService
    {
        #region Constants

        public const string FILE_TYPE = "Chemical";

        #endregion

        #region Properties

        public override string FileType => FILE_TYPE;

        #endregion

        #region Constructors

        public SapChemicalFileService(ISapChemicalServiceConfiguration config, IFileClientFactory fileClientFactory, ILog log) : base(config, fileClientFactory, log) {}

        #endregion
    }
}
