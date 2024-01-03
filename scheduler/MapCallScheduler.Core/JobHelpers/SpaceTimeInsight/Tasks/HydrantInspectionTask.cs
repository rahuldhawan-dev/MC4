using System.Linq;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCallScheduler.JobHelpers.SpaceTimeInsight.Tasks
{
    public class HydrantInspectionTask : SpaceTimeInsightMonthlyFileDumpTaskBase<HydrantInspection>
    {
        #region Fields

        private readonly IDateTimeProvider _dateTimeProvider;

        #endregion

        public HydrantInspectionTask(IRepository<HydrantInspection> repository,
            ISpaceTimeInsightJsonFileSerializer serializer, ISpaceTimeInsightFileUploadService uploadService,
            IDateTimeProvider dateTimeProvider, ILog log) : base(repository, serializer, uploadService, log)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        #region Private Methods

        protected override void UploadFile(string fileContents)
        {
            _uploadService.UploadHydrantInspections(fileContents);
        }

        protected override IQueryable<HydrantInspection> GetEntities()
        {
            return _repository.GetFromPastMonthImpl(_dateTimeProvider);
        }

        protected override string SerializeEntities(IQueryable<HydrantInspection> entities)
        {
            return _serializer.SerializeHydrantInspections(entities);
        }

        #endregion
    }
}