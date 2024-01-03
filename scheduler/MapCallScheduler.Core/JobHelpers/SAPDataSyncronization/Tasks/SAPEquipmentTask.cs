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
    public class SAPEquipmentTask : SAPEquipmentSyncronizationTaskBase<Equipment, IRepository<Equipment>>
    {
        #region Constructors

        public SAPEquipmentTask(IRepository<Equipment> repository, ISAPEquipmentRepository SAPRepository, ILog log) : base(repository, SAPRepository, log) { }

        #endregion

        #region Private Methods

        protected override SAPEquipment CreateSAPEquipment(Equipment entity)
        {
            return new SAPEquipment(entity);
        }

        protected override IEnumerable<Equipment> GetEntities()
        {
            return _repository.GetEquipmentWithSapRetryIssuesImpl();
        }

        #endregion
    }
}