using log4net;
using MapCallScheduler.Library.Configuration;
using MapCallScheduler.Library.Filesystem;
using MMSINC.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapCallScheduler.JobHelpers.SystemDeliveryEntry.DumpTasks
{
    public class SystemDeliveryEntryFileUploader : ISystemDeliveryEntryFileUploader
    {
        #region Private Members

        private readonly IFileConfigSection _config;
        private readonly IFileClient _fileClient;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILog _log;

        #endregion

        #region Constructors

        public SystemDeliveryEntryFileUploader(ISystemDeliveryEntryFileDumpServiceConfiguration config, IFileClient fileClient,
            IDateTimeProvider dateTimeProvider, ILog log)
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
                $"MapCall_{type}_{_dateTimeProvider.GetCurrentDate().ToString(CommonStringFormats.SQL_DATE_WITHOUT_PARAMETER)}.txt";
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

        public void UploadSystemDeliveryEntries(string file)
        {
            UploadFile(file, "SystemDeliveryEntries");
        }

        #endregion
    }

    public interface ISystemDeliveryEntryFileUploader
    {
        #region Abstract Methods

        void UploadSystemDeliveryEntries(string file);

        #endregion
    }
}
