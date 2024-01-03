using System.Linq;
using log4net;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCallScheduler.JobHelpers.GIS.DumpTasks
{
    public class ServiceTask : DailyGISFileDumpTaskBase<MostRecentlyInstalledService>
    {
        #region Constructors

        public ServiceTask(
            IRepository<MostRecentlyInstalledService> repository,
            IGISFileSerializer serializer,
            IGISFileUploader uploadService,
            IDateTimeProvider dateTimeProvider,
            ILog log)
            : base(
                repository,
                serializer,
                uploadService,
                dateTimeProvider,
                log) {}

        #endregion

        #region Private Methods

        protected override void UploadFile(string fileContents)
        {
            _uploadService.UploadServices(fileContents);
        }

        protected override string SerializeEntities(IQueryable<MostRecentlyInstalledService> entities)
        {
            return _serializer.Serialize(entities);
        }

        #endregion
    }
}
