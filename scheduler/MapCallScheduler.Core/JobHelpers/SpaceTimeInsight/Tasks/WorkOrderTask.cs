using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCallScheduler.JobHelpers.SpaceTimeInsight.Tasks
{
    public class WorkOrderTask : SpaceTimeInsightDailyFileDumpTaskBase<WorkOrder>
    {
        #region Private Members

        private readonly IDateTimeProvider _dateTimeProvider;

        #endregion

        #region Constructors

        public WorkOrderTask(IRepository<WorkOrder> repository, ISpaceTimeInsightJsonFileSerializer serializer,
            ISpaceTimeInsightFileUploadService uploadService, IDateTimeProvider dateTimeProvider, ILog log) : base(repository,
            serializer, uploadService, log)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        #endregion

        #region Private Methods

        protected override IQueryable<WorkOrder> GetEntities()
        {
            return _repository.GetLostWaterInPastDayImpl(_dateTimeProvider);
        }

        protected override string SerializeEntities(IQueryable<WorkOrder> coll)
        {
            return _serializer.SerializeWorkOrders(coll);
        }

        protected override void UploadFile(string file)
        {
            _uploadService.UploadWorkOrders(file);
        }

        #endregion
    }
}
