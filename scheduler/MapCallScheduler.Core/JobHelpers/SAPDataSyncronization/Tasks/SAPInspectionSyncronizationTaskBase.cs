using log4net;
using MapCall.Common.Model.Entities;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MMSINC.Data.NHibernate;

namespace MapCallScheduler.JobHelpers.SAPDataSyncronization.Tasks
{
    public abstract class SAPInspectionSyncronizationTaskBase<TEntity, TRepository> : SAPSyncronizationTaskBase<TEntity, TRepository>
        where TEntity : ISAPInspection
        where TRepository : IRepository<TEntity>
    {
        #region Properties

        protected ISAPInspectionRepository SAPRepository { get; }

        #endregion

        #region Constructors

        public SAPInspectionSyncronizationTaskBase(TRepository repository, ISAPInspectionRepository sapRepository, ILog log) : base(repository, log)
        {
            SAPRepository = sapRepository;
        }

        #endregion

        #region Private Methods

        protected override void UpdateEntityForSap(TEntity entity)
        {
            var sapInspection = SAPRepository.Save(CreateSAPInspection(entity));

            if (sapInspection.SAPNotificationNumber != null)
            {
                entity.SAPNotificationNumber = sapInspection.SAPNotificationNumber;
            }

            entity.SAPErrorCode = sapInspection.SAPErrorCode;
        }

        #endregion

        #region Abstract Methods

        protected abstract SAPInspection CreateSAPInspection(TEntity entity);

        #endregion
    }
}