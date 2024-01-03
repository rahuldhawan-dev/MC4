using log4net;
using MapCall.Common.Model.Entities;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MMSINC.Data.NHibernate;

namespace MapCallScheduler.JobHelpers.SAPDataSyncronization.Tasks
{
    public abstract class SAPEquipmentSyncronizationTaskBase<TEntity, TRepository> : SAPSyncronizationTaskBase<TEntity, TRepository>
        where TEntity : ISAPEquipment
        where TRepository : IRepository<TEntity>
    {
        #region Properties

        protected ISAPEquipmentRepository SAPRepository { get; }

        #endregion

        #region Constructors

        protected SAPEquipmentSyncronizationTaskBase(TRepository repository, ISAPEquipmentRepository sapRepository,
            ILog log) : base(repository, log)
        {
            SAPRepository = sapRepository;
        }

        #endregion

        #region Private Methods

        protected override void UpdateEntityForSap(TEntity entity)
        {
            _log.Info($"{typeof(TEntity).Name} Updating ID: {entity.SAPEquipmentId}");
            var sapEquipment = SAPRepository.Save(CreateSAPEquipment(entity));

            if (!string.IsNullOrWhiteSpace(sapEquipment.SAPEquipmentNumber))
            {
                entity.SAPEquipmentId = int.Parse(sapEquipment.SAPEquipmentNumber);
            }

            entity.SAPErrorCode = sapEquipment.SAPErrorCode;
        }

        #endregion

        #region Abstract Methods

        protected abstract SAPEquipment CreateSAPEquipment(TEntity entity);

        #endregion
    }
}