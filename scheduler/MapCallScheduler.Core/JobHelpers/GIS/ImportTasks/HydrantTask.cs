using System.Collections.Generic;
using log4net;
using MapCall.Common.Model.Entities;
using MapCallScheduler.Library.Common;
using MMSINC.Data.NHibernate;

namespace MapCallScheduler.JobHelpers.GIS.ImportTasks
{
    public class HydrantTask : GISFileImportTaskBase<Hydrant>, IDailyGISFileImportTask
    {
        #region Constants

        public const string FILE_TYPE = "hydrant import";

        #endregion

        #region Properties

        protected override string FileType => FILE_TYPE;

        #endregion

        #region Constructors

        public HydrantTask(IGISFileDownloadService downloadService, IGISFileParser parser, IRepository<Hydrant> repository, ILog log) : base(downloadService, parser, repository, log)  { }

        #endregion

        #region Private Methods

        protected override IEnumerable<GISFileParser.ParsedRecord> ParseRecords(string json)
        {
            return _parser.ParseHydrants(json);
        }

        protected override IEnumerable<FileData> DownloadFiles()
        {
            return _downloadService.DownloadHydrantFiles();
        }

        #endregion
    }
}
