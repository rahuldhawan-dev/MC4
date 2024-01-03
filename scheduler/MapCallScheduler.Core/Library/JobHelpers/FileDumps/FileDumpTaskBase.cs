using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using MapCall.Common.Utility.Scheduling;

namespace MapCallScheduler.Library.JobHelpers.FileDumps
{
    public abstract class FileDumpTaskBase<TSerializer, TUploadService> : IFileDumpTask
    {
        #region Private Members

        protected readonly TSerializer _serializer;
        protected readonly TUploadService _uploadService;

        #endregion

        #region Constructors

        public FileDumpTaskBase(TSerializer serializer, TUploadService uploadService)
        {
            _serializer = serializer;
            _uploadService = uploadService;
        }

        #endregion

        #region Abstract Methods

        protected abstract void UploadFile(string fileContents);
        public abstract void Run();

        #endregion
    }

    public abstract class FileDumpTaskBase<TSerializer, TUploadService, TEntity, TRepository> : FileDumpTaskBase<TSerializer, TUploadService>
    {
        #region Private Members

        protected readonly TRepository _repository;
        protected readonly ILog _log;

        #endregion

        #region Constructors

        protected FileDumpTaskBase(TRepository repository, TSerializer serializer, TUploadService uploadService, ILog log) : base(serializer, uploadService)
        {
            _repository = repository;
            _log = log;
        }

        #endregion

        #region Private Methods

        protected virtual bool HasDuplicatesOrGaps(IEnumerable<TEntity> entities)
        {
            return false;
        }

        /// <summary>
        /// Override this in your *Task to run any steps needed after your file's been uploaded. For example,
        /// you may want to set the status of a record indicating it was just uploaded.
        /// </summary>
        /// <param name="entities">an IEnumerable of TEntity you want to perform post processing on</param>
        protected virtual void PostProcessing(IQueryable<TEntity> entities)
        {
            // Noop
        }

        #endregion

        #region Abstract Methods

        protected abstract IQueryable<TEntity> GetEntities();
        protected abstract string SerializeEntities(IQueryable<TEntity> entities);

        #endregion

        #region Exposed Methods

        public override void Run()
        {
            var coll = GetEntities();
            var count = coll.Count();

            if (count == 0)
            {
                _log.Info("No entities to dump, skipping.");
                return;
            }

            if (HasDuplicatesOrGaps(coll))
            {
                _log.Info("Duplicates/gaps detected, exiting.");
                return;
            }

            _log.Info($"Found {count} {typeof(TEntity).Name} records to process...");

            UploadFile(SerializeEntities(coll));
            
            PostProcessing(coll);
        }

        #endregion
    }

    public interface IFileDumpTask : ITask {}
}
