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
    public class SAPSewerOpeningTask : SAPEquipmentSyncronizationTaskBase<SewerOpening, IRepository<SewerOpening>>
    {
        #region Constructors

        public SAPSewerOpeningTask(IRepository<SewerOpening> repository, ISAPEquipmentRepository SAPRepository, ILog log) : base(repository, SAPRepository, log) { }

        #endregion

        #region Private Methods

        protected override IEnumerable<SewerOpening> GetEntities()
        {
            return _repository.GetSewerOpeningsWithSapRetryIssuesImpl();
        }

        protected override SAPEquipment CreateSAPEquipment(SewerOpening entity) => new SAPEquipment(entity);

        #endregion
    }
}