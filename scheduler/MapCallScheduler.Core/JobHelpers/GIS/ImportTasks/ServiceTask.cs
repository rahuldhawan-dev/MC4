using System.Collections.Generic;
using log4net;
using MapCall.Common.Model.Entities;
using MapCallScheduler.Library.Common;
using MMSINC.Data.NHibernate;

namespace MapCallScheduler.JobHelpers.GIS.ImportTasks
{
    public class ServiceTask : GISFileImportTaskBase<Service>, IDailyGISFileImportTask
    {
        #region Constants

        public const string FILE_TYPE = "valve import";

        #endregion

        #region Properties

        protected override string FileType => FILE_TYPE;

        #endregion

        #region Constructors

        public ServiceTask(IGISFileDownloadService downloadService, IGISFileParser parser, IRepository<Service> repository, ILog log) : base(downloadService, parser, repository, log) { }

        #endregion

        #region Private Methods

        protected override IEnumerable<GISFileParser.ParsedRecord> ParseRecords(string json)
        {
            return _parser.ParseServices(json);
        }

        protected override IEnumerable<FileData> DownloadFiles()
        {
            return _downloadService.DownloadServiceFiles();
        }

        #endregion
    }
}
