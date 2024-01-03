using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCallScheduler.JobHelpers.SAPDataSyncronization.Tasks
{
    public abstract class SAPSyncronizationTaskBase : ISAPSyncronizationTask
    {
        #region Abstract Methods

        public abstract void Run();

        #endregion
    }

    public abstract class SAPSyncronizationTaskBase<TEntity,TRepository> : SAPSyncronizationTaskBase
        where TRepository : IRepository<TEntity> 
        where TEntity : ISAPEntity
    {
        #region Constants

        public const string SAP_UPDATE_FAILURE = "RETRY::UPDATE FAILURE: ";

        #endregion

        #region Private Members

        protected readonly TRepository _repository;
        protected readonly ILog _log;

        #endregion

        #region Constructors

        public SAPSyncronizationTaskBase(TRepository repository, ILog log)
        {
            _repository = repository;
            _log = log;
        }

        #endregion

        #region Abstract Methods

        // This exists because SAPEquipment takes a constructor that is either Hydrant, Valve, or SewerManhole
        protected abstract void UpdateEntityForSap(TEntity entity);
        protected abstract IEnumerable<TEntity> GetEntities();

        #endregion

        #region Exposed Methods

        public override void Run()
        {
            _log.Info($"Processing SAP {typeof(TEntity).Name} with issues...");
            var entities = GetEntities().ToList();
            _log.Info($"{entities.Count} Records to process.");
            int count = 0, errorCount = 0;
            foreach (var entity in entities)
            {
                count++;
                try
                {
                    UpdateEntityForSap(entity);
                }
                catch (Exception ex)
                {
                    errorCount++;
                    entity.SAPErrorCode = SAP_UPDATE_FAILURE + ex.Message;
                    _log.Error($"A runtime error occurred syncing a(n) {typeof(TEntity).Name} to SAP", ex);
                }
                finally
                {
                    _repository.Save(entity);
                }
            }
            _log.Info($"Completed Processing ({count}/{entities.Count}) SAP {typeof(TEntity).Name}s. {errorCount} Exceptions");
        }

        #endregion
    }
}