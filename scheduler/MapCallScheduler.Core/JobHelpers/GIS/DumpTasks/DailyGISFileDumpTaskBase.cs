using System.Linq;
using log4net;
using MapCall.Common.Model.Entities;
using MapCallScheduler.Library.JobHelpers.FileDumps;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCallScheduler.JobHelpers.GIS.DumpTasks
{
    public abstract class DailyGISFileDumpTaskBase<TEntity> : FileDumpTaskBase<IGISFileSerializer, IGISFileUploader, TEntity, IRepository<TEntity>>, IDailyGISFileDumpTask
        where TEntity : IThingWithShadow
    {
        #region Private Members

        protected IDateTimeProvider _dateTimeProvider;

        #endregion

        #region Constructors

        public DailyGISFileDumpTaskBase(IRepository<TEntity> repository, IGISFileSerializer serializer,
            IGISFileUploader uploadService, IDateTimeProvider dateTimeProvider, ILog log) : base(repository, serializer,
            uploadService, log)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        #endregion

        #region Private Methods

        protected override IQueryable<TEntity> GetEntities()
        {
            var now = _dateTimeProvider.GetCurrentDate();
            var twentyfourhoursago = now.AddDays(-1);
            return _repository.Linq
                              .Where(x =>
                                   x.UpdatedAt >= twentyfourhoursago &&
                                   x.UpdatedAt < now);
        }

        #endregion
    }
}
