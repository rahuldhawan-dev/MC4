using System.IO;
using log4net;
using MapCallScheduler.Library.Configuration;
using MapCallScheduler.Library.Filesystem;
using MMSINC.Utilities;

namespace MapCallScheduler.JobHelpers.GIS
{
    public class GISFileUploader : IGISFileUploader
    {
        #region Private Members

        private readonly IFileConfigSection _config;
        private readonly IFileClient _fileClient;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILog _log;

        #endregion

        #region Constructors

        public GISFileUploader(IGISFileDumpServiceConfiguration config, IFileClient fileClient, IDateTimeProvider dateTimeProvider, ILog log)
        {
            _config = config.FileConfig;
            _fileClient = fileClient;
            _dateTimeProvider = dateTimeProvider;
            _log = log;
        }

        #endregion

        private string GetFilename(string type)
        {
            return
                $"MapCall_{type}_{_dateTimeProvider.GetCurrentDate().ToString(CommonStringFormats.SQL_DATE_WITHOUT_PARAMETER)}.json";
        }

        private void UploadFile(string file, string type)
        {
            if (_config == null)
            {
                _log.Info($"_config is null!");
                return;
            }

            if (_fileClient == null)
            {
                _log.Info("_fileClient is null!");
                return;
            }

            var filePath = Path.Combine(_config.WorkingDirectory, GetFilename(type));

            _log.Info($"Writing {type} file data to {filePath}...");

            _fileClient.WriteFile(filePath, file);
        }

        #region Exposed Methods

        public void UploadHydrants(string file)
        {
            UploadFile(file, "Hydrants");
        }

        public void UploadValves(string file)
        {
            UploadFile(file, "Valves");
        }

        public void UploadSewerOpenings(string file)
        {
            UploadFile(file, "SewerOpenings");
        }

        public void UploadServices(string file)
        {
            UploadFile(file, "Services");
        }

        public void UploadAsBuiltImages(string file)
        {
            UploadFile(file, "AsBuiltImages");
        }

        #endregion
    }

    public interface IGISFileUploader
    {
        #region Abstract Methods

        void UploadHydrants(string file);
        void UploadValves(string file);
        void UploadSewerOpenings(string file);
        void UploadServices(string file);
        void UploadAsBuiltImages(string file);

        #endregion
    }
}
