using log4net;
using MapCallScheduler.Library.Filesystem;
using MapCallScheduler.Library.JobHelpers.Common;

namespace MapCallScheduler.Library.JobHelpers.Sap
{
    /// <summary>
    /// See ISapFileService.ca
    /// </summary>
    public abstract class SapFileServiceBase<TConfiguration> : FileDownloadServiceBase<TConfiguration>, ISapFileService
        where TConfiguration : ISapServiceConfiguration
    {
        #region Properties

        public override string FilePattern => $"MapCall_{FileType}Master_*";

        #endregion

        #region Constructors

        public SapFileServiceBase(TConfiguration config, IFileClientFactory fileClientFactory, ILog log) : base(config, fileClientFactory, log) { }

        #endregion
    }
}