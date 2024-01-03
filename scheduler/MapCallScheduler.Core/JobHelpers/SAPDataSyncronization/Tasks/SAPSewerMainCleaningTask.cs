using System.Collections.Generic;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MMSINC.Data.NHibernate;

namespace MapCallScheduler.JobHelpers.SAPDataSyncronization.Tasks
{
    public class SAPSewerMainCleaningTask : SAPInspectionSyncronizationTaskBase<SewerMainCleaning, IRepository<SewerMainCleaning>>
    {
        #region Constructors

        public SAPSewerMainCleaningTask(IRepository<SewerMainCleaning> repository, ISAPInspectionRepository sapRepository, ILog log) : base(repository, sapRepository, log) { }

        #endregion

        #region Private Methods

        protected override IEnumerable<SewerMainCleaning> GetEntities()
        {
            return _repository.GetSewerMainCleaningsWithSapRetryIssuesImpl();
        }

        protected override SAPInspection CreateSAPInspection(SewerMainCleaning entity) => new SAPInspection(entity);

        #endregion
    }
}