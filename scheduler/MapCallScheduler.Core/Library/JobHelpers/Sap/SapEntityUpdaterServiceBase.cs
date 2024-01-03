using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using log4net;
using MapCallScheduler.Library.Common;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCallScheduler.Library.JobHelpers.Sap
{
    public abstract class SapEntityUpdaterServiceBase<TFileRecord, TParser, TEntity, TRepository> : ISapEntityUpdaterService
        where TFileRecord : new()
        where TParser : IFileParser<TFileRecord>
        where TRepository : IRepository<TEntity>
    {
        #region Private Members

        protected readonly TParser _parser;
        protected readonly TRepository _repository;
        protected readonly ILog _log;

        #endregion

        #region Constructors

        protected SapEntityUpdaterServiceBase(TParser parser, TRepository repository, ILog log)
        {
            _parser = parser;
            _repository = repository;
            _log = log;
        }

        #endregion

        #region Private Methods

        protected virtual void MaybeMapField<TMapTo, TValue>(TMapTo to, string fieldName, TValue oldValue, TValue newValue)
        {
            if ((oldValue == null && newValue != null) ||
                (oldValue != null && !oldValue.Equals(newValue)))
            {
                _log.Debug($"Changing field '{fieldName}' from '{oldValue}' to '{newValue}'...");
                to.SetPropertyValueByName(fieldName, newValue);
            }
        }

        private void MapField<TMapTo, TMapFrom, TValue>(TMapTo to, TMapFrom from, Expression<Func<TMapTo, TValue>> fn)
        {
            var fieldName = Expressions.GetMember(fn).Name;

            MaybeMapField(to, fieldName, fn.Compile()(to), (TValue)from.GetPropertyValueByName(fieldName));
        }

        protected void MapField<TMapTo, TMapFrom>(TMapTo to, TMapFrom from, Expression<Func<TMapTo, string>> fn)
        {
            MapField<TMapTo, TMapFrom, string>(to, from, fn);
        }

        protected void MapField<TMapTo, TMapFrom>(TMapTo to, TMapFrom from, Expression<Func<TMapTo, DateTime?>> fn)
        {
            MapField<TMapTo, TMapFrom, DateTime?>(to, from, fn);
        }

        #endregion

        #region Abstract Methods

        protected abstract void MapRecord(TEntity entity, TFileRecord record, int currentLine);

        protected abstract TEntity FindOrCreateEntity(TFileRecord record, int currentLine);

        protected abstract void LogRecord(TFileRecord record);

        #endregion
        
        protected virtual void SecondPass(List<TEntity> mappedEntities)
        {
            // noop
        }

        #region Exposed Methods

        public virtual void Process(FileData sapFile)
        {
            var entities = new List<TEntity>();
            var currentLine = 2;
            _log.Info($"Parsing file '{sapFile.Filename}'...");

            foreach (var record in _parser.Parse(sapFile))
            {
                LogRecord(record);
                var entity = FindOrCreateEntity(record, currentLine);
                MapRecord(entity, record, currentLine++);
                entities.Add(entity);
            }

            SecondPass(entities);

            // Employee updater needs to use the Repository.Save method that takes an IEnumerable
            // due to saving Employee records where the ReportsTo property might also be a new Employee record.
            _repository.Save(entities);
        }

        #endregion
    }
}