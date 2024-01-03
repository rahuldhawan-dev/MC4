using log4net;
using MapCallScheduler.Library.Filesystem;
using MapCallScheduler.Library.JobHelpers.Sap;

namespace MapCallScheduler.JobHelpers.SapMaterial
{
    public class SapMaterialFileService : SapFileServiceBase<ISapMaterialServiceConfiguration>, ISapMaterialFileService
    {
        #region Constants

        public const string FILE_TYPE = "Material";

        #endregion

        #region Properties

        public override string FileType => FILE_TYPE;

        #endregion

        #region Constructors

        public SapMaterialFileService(ISapMaterialServiceConfiguration config, IFileClientFactory fileClientFactory, ILog log) : base(config, fileClientFactory, log) {}

        #endregion
    }
}