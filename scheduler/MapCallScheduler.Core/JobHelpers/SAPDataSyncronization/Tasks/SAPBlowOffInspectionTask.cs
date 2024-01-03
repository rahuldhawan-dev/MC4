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
    public class SAPBlowOffInspectionTask : SAPInspectionSyncronizationTaskBase<BlowOffInspection, IRepository<BlowOffInspection>>
    {
        #region Constructors

        public SAPBlowOffInspectionTask(IRepository<BlowOffInspection> repository, ISAPInspectionRepository inspectionRepository, ILog log) : base(repository, inspectionRepository, log) { }

        #endregion
        
        #region Private Methods

        protected override SAPInspection CreateSAPInspection(BlowOffInspection entity) => new SAPInspection(entity);

        protected override IEnumerable<BlowOffInspection> GetEntities()
        {
            return _repository.GetBlowOffInspectionsWithSapRetryIssuesImpl();
        }

        #endregion
    }
}