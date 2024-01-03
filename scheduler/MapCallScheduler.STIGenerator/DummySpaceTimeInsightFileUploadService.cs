using System.IO;
using log4net;
using MapCallScheduler.JobHelpers.SpaceTimeInsight;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Utilities;

namespace MapCallScheduler.STIGenerator
{
    public class DummySpaceTimeInsightFileUploadService : ISpaceTimeInsightFileUploadService
    {
        #region Constants

        public const string BASE_DIR = "d:\\Solutions\\nogit\\STI Dumps\\";

        #endregion

        #region Private Members

        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILog _log;

        #endregion

        #region Constructors

        public DummySpaceTimeInsightFileUploadService(IDateTimeProvider dateTimeProvider, ILog log)
        {
            _dateTimeProvider = dateTimeProvider;
            _log = log;
        }

        #endregion

        #region Exposed Methods

        public void Upload(string file, string type)
        {
            var path = Path.Combine(BASE_DIR,
                $"{type}-{_dateTimeProvider.GetCurrentDate().MillisecondsSinceEpoch()}.json");

            _log.Info($"Writing {type} file '{path}'...");
            
            File.WriteAllText(path, file);
        }

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
    }
}