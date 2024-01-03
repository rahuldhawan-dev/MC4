using System.Linq;

namespace MapCallScheduler.Library.JobHelpers.Sap
{
    /// <summary>
    /// See ISapFileProcessingService.cs
    /// </summary>
    public abstract class SapFileProcessingServiceBase<TFileService, TUpdaterService> : ISapFileProcessingService
        where TFileService : ISapFileService
        where TUpdaterService : ISapEntityUpdaterService
    {
        #region Private Members

        protected readonly TFileService _fileService;
        protected readonly TUpdaterService _updaterService;

        #endregion

        #region Constructors

        protected SapFileProcessingServiceBase(TFileService fileService, TUpdaterService updaterService)
        {
            _fileService = fileService;
            _updaterService = updaterService;
        }

        #endregion

        #region Exposed Methods

        public virtual void Process()
        {
            foreach (var file in _fileService.GetAllFiles())
            {
                _updaterService.Process(file);
                _fileService.DeleteFile(file.Filename);
            }
        }

        #endregion
    }
}
