using log4net;
using MapCall.Common.Utility.Scheduling;
using MapCallScheduler.Library.JobHelpers.FileDumps;
using MMSINC.Data.NHibernate;

namespace MapCallScheduler.JobHelpers.SpaceTimeInsight.Tasks
{

    public abstract class SpaceTimeInsightFileDumpTaskBase<TEntity, TRepository> : FileDumpTaskBase<ISpaceTimeInsightJsonFileSerializer, ISpaceTimeInsightFileUploadService, TEntity, TRepository>, ISpaceTimeInsightFileDumpTask
    {
        protected SpaceTimeInsightFileDumpTaskBase(TRepository repository, ISpaceTimeInsightJsonFileSerializer serializer, ISpaceTimeInsightFileUploadService uploadService, ILog log) : base(repository, serializer, uploadService, log) { }
    }

    public abstract class SpaceTimeInsightFileDumpTaskBase<TEntity> : SpaceTimeInsightFileDumpTaskBase<TEntity, IRepository<TEntity>>
    {
        #region Constructors

        public SpaceTimeInsightFileDumpTaskBase(IRepository<TEntity> repository, ISpaceTimeInsightJsonFileSerializer serializer, ISpaceTimeInsightFileUploadService uploadService, ILog log) : base(repository, serializer, uploadService, log) {}

        #endregion
    }

    public abstract class SpaceTimeInsightDailyFileDumpTaskBase<TEntity, TRepository> : SpaceTimeInsightFileDumpTaskBase<TEntity, TRepository>
    {
        #region Constructors

        public SpaceTimeInsightDailyFileDumpTaskBase(TRepository repository, ISpaceTimeInsightJsonFileSerializer serializer, ISpaceTimeInsightFileUploadService uploadService, ILog log) : base(repository, serializer, uploadService, log) {}

        #endregion
    }

    public abstract class SpaceTimeInsightDailyFileDumpTaskBase<TEntity> : SpaceTimeInsightDailyFileDumpTaskBase<TEntity, IRepository<TEntity>>
    {
        #region Constructors

        public SpaceTimeInsightDailyFileDumpTaskBase(IRepository<TEntity> repository, ISpaceTimeInsightJsonFileSerializer serializer, ISpaceTimeInsightFileUploadService uploadService, ILog log) : base(repository, serializer, uploadService, log) {}

        #endregion
    }

    public abstract class SpaceTimeInsightMonthlyFileDumpTaskBase<TEntity, TRepository> : SpaceTimeInsightFileDumpTaskBase<TEntity, TRepository>
    {
        #region Constructors

        public SpaceTimeInsightMonthlyFileDumpTaskBase(TRepository repository, ISpaceTimeInsightJsonFileSerializer serializer, ISpaceTimeInsightFileUploadService uploadService, ILog log) : base(repository, serializer, uploadService, log) {}

        #endregion
    }

    public abstract class SpaceTimeInsightMonthlyFileDumpTaskBase<TEntity> : SpaceTimeInsightMonthlyFileDumpTaskBase<TEntity, IRepository<TEntity>>
    {
        #region Constructors

        public SpaceTimeInsightMonthlyFileDumpTaskBase(IRepository<TEntity> repository, ISpaceTimeInsightJsonFileSerializer serializer, ISpaceTimeInsightFileUploadService uploadService, ILog log) : base(repository, serializer, uploadService, log) {}

        #endregion
    }

    public interface ISpaceTimeInsightFileDumpTask : IFileDumpTask {}
}
