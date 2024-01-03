using System.IO;
using log4net;
using MapCallScheduler.Library.Configuration;
using MapCallScheduler.Library.Filesystem;
using MMSINC.Utilities;

namespace MapCallScheduler.JobHelpers.NonRevenueWater
{
    public class NonRevenueWaterEntryFileUploader : INonRevenueWaterEntryFileUploader
    {
        #region Private Members

        private readonly IFileConfigSection _config;
        private readonly IFileClient _fileClient;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILog _log;

        #endregion

        #region Constructors

        public NonRevenueWaterEntryFileUploader(INonRevenueWaterEntryFileDumpServiceConfiguration config,
            IFileClient fileClient, IDateTimeProvider dateTimeProvider, ILog log)
        {
            _config = config.FileConfig;
            _fileClient = fileClient;
            _dateTimeProvider = dateTimeProvider;
            _log = log;
        }

        #endregion

        #region Private Methods

        private void UploadFile(string file, string type)
        {
            if (_config == null)
            {
                _log.Info("_config is null.");
                return;
            }

            if (_fileClient == null)
            {
                _log.Info("_fileClient is null.");
                return;
            }

            var filePath = Path.Combine(_config.WorkingDirectory, GetFilename(type));

            _log.Info($"Writing {type} file data to {filePath}...");

            _fileClient.WriteFile(filePath, file);
        }

        private string GetFilename(string type) =>
            $"MapCall_{type}_{_dateTimeProvider.GetCurrentDate().ToString(CommonStringFormats.SQL_DATE_WITHOUT_PARAMETER)}.txt";

        #endregion

        #region Exposed Methods

        public void UploadNonRevenueWaterEntries(string file) => UploadFile(file, "AccountedForLosses");

        #endregion
    }
}
