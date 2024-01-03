using System;
using System.Collections.Generic;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MMSINC.Data.NHibernate;

namespace MapCallScheduler.JobHelpers.SAPDataSyncronization.Tasks
{
    public class SAPHydrantInspectionTask : SAPInspectionSyncronizationTaskBase<HydrantInspection, IRepository<HydrantInspection>>
    {
        #region Constructors

        public SAPHydrantInspectionTask(IRepository<HydrantInspection> repository, ISAPInspectionRepository sapRepository, ILog log) : base(repository, sapRepository, log) { }

        #endregion

        #region Private Methods

        protected override IEnumerable<HydrantInspection> GetEntities()
        {
            return _repository.GetHydrantInspectionsWithSapRetryIssuesImpl();
        }

        protected override SAPInspection CreateSAPInspection(HydrantInspection entity) => new SAPInspection(entity);

        #endregion
    }
}