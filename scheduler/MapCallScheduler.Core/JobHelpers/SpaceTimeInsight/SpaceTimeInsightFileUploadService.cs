using System.IO;
using log4net;
using MapCallScheduler.Library.Ftp;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Utilities;

namespace MapCallScheduler.JobHelpers.SpaceTimeInsight
{
    public class SpaceTimeInsightFileUploadService : ISpaceTimeInsightFileUploadService
    {
        #region Private Members

        private readonly IFtpClientFactory _ftpFactory;
        private readonly ISpaceTimeInsightFileDumpServiceConfiguration _config;
        private readonly ILog _log;
        private readonly IDateTimeProvider _dateTimeProvider;

        #endregion

        #region Constructors

        public SpaceTimeInsightFileUploadService(SpaceTimeInsightFileUploadServiceArgs args)
        {
            _log = args.Log;
            _config = args.Config;
            _ftpFactory = args.FtpFactory;
            _dateTimeProvider = args.DateTimeProvider;
        }

        #endregion

        #region Private Methods

        private void Upload(string file, string type)
        {
            using (var conn = _ftpFactory.FromConfig(_config.FtpConfig))
            {
                var fileName = $"{type}-{_dateTimeProvider.GetCurrentDate().SecondsSinceEpoch()}.json";
                var path =
                    $"{_config.FtpConfig.WorkingDirectory}/{fileName}";
                _log.Info($"Uploading {type} file to {path}");

                using (var stream = conn.OpenWrite(path))
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(file);
                    writer.Flush();
                }
            }
        }

        #endregion

        #region Exposed Methods

        public void UploadWorkOrders(string file)
        {
            Upload(file, "workorder");
        }

        public void UploadMainBreaks(string file)
        {
            Upload(file, "mainbreak");
        }

        public void UploadHydrantInspections(string file)
        {
            Upload(file, "hydrant-inspection");
        }

        public void UploadTankLevelData(string file)
        {
            Upload(file, "tank-level");
        }

        public void UploadInterconnectData(string file)
        {
            Upload(file, "interconnect");
        }

        #endregion

        #region Nested Type: SpaceTimeInsightFileUploadServiceArgs

        public class SpaceTimeInsightFileUploadServiceArgs
        {
            #region Properties

            public IDateTimeProvider DateTimeProvider { get; }

            public IFtpClientFactory FtpFactory { get; }

            public ISpaceTimeInsightFileDumpServiceConfiguration Config { get; }

            public ILog Log { get; }

            #endregion

            #region Constructors

            public SpaceTimeInsightFileUploadServiceArgs(ILog log, ISpaceTimeInsightFileDumpServiceConfiguration config, IFtpClientFactory ftpFactory, IDateTimeProvider dateTimeProvider)
            {
                Log = log;
                Config = config;
                FtpFactory = ftpFactory;
                DateTimeProvider = dateTimeProvider;
            }

            #endregion
        }

        #endregion
    }

    public interface ISpaceTimeInsightFileUploadService
    {
        #region Abstract Methods

        void UploadWorkOrders(string file);
        void UploadMainBreaks(string file);
        void UploadHydrantInspections(string file);
        void UploadTankLevelData(string file);
        void UploadInterconnectData(string file);

        #endregion
    }
}