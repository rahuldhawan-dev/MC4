using System.Collections.Generic;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MMSINC.Data.NHibernate;

namespace MapCallScheduler.JobHelpers.SAPDataSyncronization.Tasks
{
    public class SAPValveInspectionTask : SAPInspectionSyncronizationTaskBase<ValveInspection, IRepository<ValveInspection>>
    {
        #region Constructors

        public SAPValveInspectionTask(IRepository<ValveInspection> repository, ISAPInspectionRepository sapRepository, ILog log) : base(repository, sapRepository, log) { }

        #endregion

        #region Private Methods

        protected override IEnumerable<ValveInspection> GetEntities()
        {
            return _repository.GetValveInspectionsWithSapRetryIssuesImpl();
        }

        protected override SAPInspection CreateSAPInspection(ValveInspection entity) => new SAPInspection(entity);

        #endregion
    }
}