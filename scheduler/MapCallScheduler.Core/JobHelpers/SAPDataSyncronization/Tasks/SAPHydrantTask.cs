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
    public class SAPHydrantTask : SAPEquipmentSyncronizationTaskBase<Hydrant, IRepository<Hydrant>>
    {
        #region Constructors

        public SAPHydrantTask(IRepository<Hydrant> repository, ISAPEquipmentRepository SAPRepository, ILog log) : base(repository, SAPRepository, log) { }

        #endregion

        #region Private Methods

        protected override IEnumerable<Hydrant> GetEntities()
        {
            return _repository.GetHydrantsWithSapRetryIssuesImpl();
        }

        protected override SAPEquipment CreateSAPEquipment(Hydrant entity) => new SAPEquipment(entity);

        #endregion
    }
}