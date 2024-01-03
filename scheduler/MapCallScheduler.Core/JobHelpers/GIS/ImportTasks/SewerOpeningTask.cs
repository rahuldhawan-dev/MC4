using System.Collections.Generic;
using log4net;
using MapCall.Common.Model.Entities;
using MapCallScheduler.Library.Common;
using MMSINC.Data.NHibernate;

namespace MapCallScheduler.JobHelpers.GIS.ImportTasks
{
    public class SewerOpeningTask : GISFileImportTaskBase<SewerOpening>, IDailyGISFileImportTask
    {
        #region Constants

        public const string FILE_TYPE = "sewer opening import";

        #endregion

        #region Properties

        protected override string FileType => FILE_TYPE;

        #endregion

        #region Constructors

        public SewerOpeningTask(IGISFileDownloadService downloadService, IGISFileParser parser, IRepository<SewerOpening> repository, ILog log) : base(downloadService, parser, repository, log) { }

        #endregion

        #region Private Methods

        protected override IEnumerable<GISFileParser.ParsedRecord> ParseRecords(string json)
        {
            return _parser.ParseSewerOpenings(json);
        }

        protected override IEnumerable<FileData> DownloadFiles()
        {
            return _downloadService.DownloadSewerOpeningFiles();
        }

        #endregion
    }
}
