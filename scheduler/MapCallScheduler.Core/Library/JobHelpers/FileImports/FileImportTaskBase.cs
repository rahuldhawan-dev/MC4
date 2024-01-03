using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using log4net;
using MapCall.Common.Utility.Scheduling;
using MapCallScheduler.Library.Common;
using MapCallScheduler.Library.JobHelpers.Common;
using MMSINC.Data;
using MMSINC.Data.NHibernate;

namespace MapCallScheduler.Library.JobHelpers.FileImports
{
    public abstract class FileImportTaskBase<TDownloadService, TParser, TParsedRecord, TEntity, TRepository> : IFileImportTask
        where TDownloadService : IFileDownloadService
        where TRepository : IRepository<TEntity>
        where TEntity : class, IEntity, new()
    {
        #region Private Members

        protected readonly TDownloadService _downloadService;
        protected readonly TParser _parser;
        protected readonly TRepository _repository;
        protected readonly ILog _log;

        #endregion

        #region Abstract Properties

        protected abstract string FileType { get; }

        #endregion

        #region Constructors

        public FileImportTaskBase(TDownloadService downloadService, TParser parser, TRepository repository, ILog log)
        {
            _downloadService = downloadService;
            _parser = parser;
            _repository = repository;
            _log = log;
        }

        #endregion

        #region Private Methods

        protected virtual void Save(TEntity entity)
        {
            _repository.Save(entity);
        }

        protected virtual TEntity MapToEntity(TEntity entity, TParsedRecord update)
        {
            entity = entity ?? new TEntity();
            MapFields(entity, update);

            return entity;
        }

        protected virtual void DeleteFile(string filePath)
        {
            _downloadService.DeleteFile(filePath);
        }

        #endregion

        #region Abstract Methods

        protected abstract IEnumerable<FileData> DownloadFiles();
        protected abstract IEnumerable<TParsedRecord> ParseRecords(string json);
        protected abstract TEntity Find(TParsedRecord update);
        protected abstract void MapFields(TEntity entity, TParsedRecord update);

        #endregion

        #region Exposed Methods

        public void Run()
        {
            var files = DownloadFiles();

            if (files == null)
            {
                _log.Info($"DownloadFiles() returned null...");
                return;
            }

            _log.Info($"Found {files.Count()} {FileType} files...");

            foreach (var file in files)
            {
                _log.Info($"Processing {FileType} file '{file.Filename}'...");

                var sw = new Stopwatch();
                var updates = ParseRecords(file.Content).ToArray();

                _log.Info($"Found {updates.Length} updates in file '{file.Filename}'...");

                sw.Start();

                for (var i = 0; i < updates.Length;)
                {
                    var update = updates[i];
                    var entity = Find(update);

                    if (entity == null)
                    {
                        _log.Info($"Creating new {typeof(TEntity).Name} ({++i} of {updates.Length})...");
                    }
                    else
                    {
                        _log.Info(
                            $"Processing {typeof(TEntity).Name} {entity.Id} ({++i} of " +
                            $"{updates.Length})...");
                    }

                    var per = sw.ElapsedMilliseconds / i;
                    var remaining = per * (updates.Length - i);
                    _log.Info($"Elapsed {sw.Elapsed}, Per {TimeSpan.FromMilliseconds(per)}, Remaining {TimeSpan.FromMilliseconds(remaining)}");

                    Save(MapToEntity(entity, update));
                }

                DeleteFile(file.Filename);
            }
        }

        #endregion
    }

    public interface IFileImportTask : ITask {}
}
