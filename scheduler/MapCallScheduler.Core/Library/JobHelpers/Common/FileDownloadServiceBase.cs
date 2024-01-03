using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using MapCallScheduler.Library.Common;
using MapCallScheduler.Library.Configuration;
using MapCallScheduler.Library.Filesystem;

namespace MapCallScheduler.Library.JobHelpers.Common
{
    public abstract class FileDownloadServiceBase<TConfiguration> : IFileDownloadService
        where TConfiguration : IFileServiceConfiguration
    {
        #region Private Members

        protected readonly ILog _log;
        protected readonly TConfiguration _config;
        protected readonly IFileClientFactory _fileClientFactory;

        #endregion

        #region Abstract Properties

        public abstract string FileType { get; }
        public abstract string FilePattern { get; }

        #endregion

        #region Constructors

        protected FileDownloadServiceBase(TConfiguration config, IFileClientFactory fileClientFactory, ILog log)
        {
            _config = config;
            _fileClientFactory = fileClientFactory;
            _log = log;
        }

        #endregion

        #region Exposed Methods

        public IEnumerable<FileData> GetAllFiles()
        {
            var conn = _fileClientFactory.Build();
            var path = _config.FileConfig.WorkingDirectory;
            _log.Info($"Getting listing for path '{path}'...");

            foreach (var item in conn.GetListing(path, FilePattern).OrderBy(f => f.FullName))
            {
                _log.Info($"Downloading file '{item.FullName}'...");

                yield return conn.DownloadFile(item.FullName);
            }
        }

        public void DeleteFile(string file)
        {
            if (!_config.FileConfig.MakeChanges)
            {
                _log.Info($"Refusing to delete file '{file}' because MakeChanges is false...");
                return;
            }

            try
            {
                _log.Info($"Deleting file '{file}'...");
                var conn = _fileClientFactory.Build();
                conn.DeleteFile(file);
            }
            catch (Exception e)
            {
                throw new Exception($"Error deleting file '{file}'", e);
            }
        }

        #endregion
    }

    /// <summary>
    /// Represents the object responsible for enumerating files on the remote server and deleting them.  You
    /// should inherit from SapFileServiceBase and provide an interface which inherits from this one.
    /// </summary>
    public interface IFileDownloadService
    {
        #region Abstract Methods

        IEnumerable<FileData> GetAllFiles();
        void DeleteFile(string foo);

        #endregion
    }
}
