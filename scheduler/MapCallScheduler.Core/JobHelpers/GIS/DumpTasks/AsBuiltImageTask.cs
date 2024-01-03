using log4net;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using System.Linq;

namespace MapCallScheduler.JobHelpers.GIS.DumpTasks
{
    public class AsBuiltImageTask : DailyGISFileDumpTaskBase<AsBuiltImage>
    {
        #region Constructors

        public AsBuiltImageTask(IRepository<AsBuiltImage> repository, IGISFileSerializer serializer,
            IGISFileUploader uploadService, IDateTimeProvider dateTimeProvider, ILog log) : base(repository, serializer,
            uploadService, dateTimeProvider, log)
        {
        }

        #endregion

        protected override string SerializeEntities(IQueryable<AsBuiltImage> entities)
        {
            return _serializer.Serialize(entities); 
        }

        protected override void UploadFile(string fileContents)
        {
            _uploadService.UploadAsBuiltImages(fileContents);
        }
    }
}
