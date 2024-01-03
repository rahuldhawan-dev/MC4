using System.Collections.Generic;
using MapCallScheduler.JobHelpers.GIS.DownloadServices;
using MapCallScheduler.Library.Common;
using MapCallScheduler.Library.JobHelpers.Common;

namespace MapCallScheduler.JobHelpers.GIS
{
    public class GISFileDownloadService : IGISFileDownloadService
    {
        #region Private Members

        private readonly IGISFileDownloadServiceServiceFactory _factory;

        #endregion

        #region Constructors

        public GISFileDownloadService(IGISFileDownloadServiceServiceFactory factory)
        {
            _factory = factory;
        }

        #endregion

        #region Exposed Methods

        public IEnumerable<FileData> DownloadHydrantFiles()
        {
            return _factory.BuildHydrantDownloadService().GetAllFiles();
        }

        public IEnumerable<FileData> DownloadSewerOpeningFiles()
        {
            return _factory.BuildSewerOpeningDownloadService().GetAllFiles();
        }

        public IEnumerable<FileData> DownloadValveFiles()
        {
            return _factory.BuildValveDownloadService().GetAllFiles();
        }

        public IEnumerable<FileData> DownloadServiceFiles()
        {
            return _factory.BuildServiceDownloadService().GetAllFiles();
        }

        public IEnumerable<FileData> GetAllFiles()
        {
            throw new System.NotImplementedException();
        }

        public void DeleteFile(string filePath)
        {
            _factory.BuildHydrantDownloadService().DeleteFile(filePath);
        }

        #endregion
    }

    public interface IGISFileDownloadService : IFileDownloadService
    {
        #region Abstract Methods

        IEnumerable<FileData> DownloadHydrantFiles();

        IEnumerable<FileData> DownloadSewerOpeningFiles();

        IEnumerable<FileData> DownloadValveFiles();

        IEnumerable<FileData> DownloadServiceFiles();

        #endregion
    }
}
