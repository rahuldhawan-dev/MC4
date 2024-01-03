using System.Collections.Generic;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MMSINC.Data.NHibernate;

namespace MapCallScheduler.JobHelpers.SAPDataSyncronization.Tasks
{
    public class SAPValveTask : SAPEquipmentSyncronizationTaskBase<Valve, IRepository<Valve>>
    {
        #region Constructors

        public SAPValveTask(IRepository<Valve> repository, ISAPEquipmentRepository SAPRepository, ILog log) : base(repository, SAPRepository, log) { }

        #endregion

        #region Private Methods

        protected override IEnumerable<Valve> GetEntities()
        {
            return _repository.GetValvesWithSapRetryIssuesImpl();
        }

        protected override SAPEquipment CreateSAPEquipment(Valve entity) => new SAPEquipment(entity);

        #endregion
    }
}