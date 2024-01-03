using System;
using System.Collections.Generic;
using MMSINC.Data.NHibernate;
using MMSINC.Data.V2;

namespace MapCallImporter.Common
{
    public abstract class ExcelRecordItemHelperBase<TEntity>
    {
        #region Private Members

        protected Dictionary<Type, Func<int, IUnitOfWork, object>> _memoizedLookupDictionary;

        #endregion

        #region Abstract Properties

        public abstract IEnumerable<string> LastErrors { get; }

        #endregion

        #region Constructors

        public ExcelRecordItemHelperBase()
        {
            _memoizedLookupDictionary = new Dictionary<Type, Func<int, IUnitOfWork, object>>();
        }

        #endregion

        #region Private Methods

        protected TRef MemoizedLookup<TRef>(IUnitOfWork uow, int id)
            where TRef : class
        {
            Func<int, IUnitOfWork, object> lookupFn;
            var refType = typeof(TRef);
            var refTypeName = refType.Name;

            if (_memoizedLookupDictionary.ContainsKey(refType))
            {
                lookupFn = _memoizedLookupDictionary[refType];
            }
            else
            {
                _memoizedLookupDictionary.Add(refType, lookupFn = FuncExtensions.MemoizeExtra<int, IUnitOfWork, TRef>(LoadEntityRef<TRef>));
            }

            return (TRef)lookupFn(id, uow);
        }

        #endregion

        #region Abstract Methods

        protected abstract void OnFailedRequirement(string message);
        protected abstract TRef LoadEntityRef<TRef>(int id, IUnitOfWork uow);

        #endregion

        #region Exposed Methods

        public virtual void AddFailure(string message)
        {
            OnFailedRequirement(message);
        }

        /// <summary>
        /// Fails on null or whitespace for 'value', otherwise returns 'value'.
        /// </summary>
        public virtual string RequiredStringValue(string value, int currentIndex, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                OnFailedRequirement($"{typeof(TEntity).Name} at row {currentIndex} has no value for '{fieldName}'.");
            }

            return value;
        }

        public virtual int? RequiredIntValue(int? value, int currentIndex, string fieldName)
        {
            if (!value.HasValue)
            {
                OnFailedRequirement($"{typeof(TEntity).Name} at row {currentIndex} has no value for '{fieldName}'.");
            }

            return value;
        }

        public decimal? RequiredDecimalValue(decimal? value, int index, string fieldName)
        {
            if (!value.HasValue)
            {
                OnFailedRequirement($"{typeof(TEntity).Name} at row {index} has no value for '{fieldName}'.");
            }

            return value;
        }

        /// <summary>
        /// Ensures that a given id of a given entity class TRef exists in the database, fails if it does not.
        /// </summary>
        /// <typeparam name="TRef">Type of entity class being looked up</typeparam>
        public virtual TRef RequiredEntityRef<TRef>(IUnitOfWork uow, int id, int currentIndex, string fieldName)
            where TRef : class
        {
            //var entityRef = uow.GetRepository<TRef>().Find(id);
            var entityRef = MemoizedLookup<TRef>(uow, id);

            if (entityRef == null)
            {
                OnFailedRequirement($"{typeof(TEntity).Name} at row {currentIndex} has {fieldName} {id} which was not found in the database.");
            }

            return entityRef;
        }

        /// <summary>
        /// Similar to the other overload, except if the nullable int id is null then default(TRef) is returned no failure.
        /// </summary>
        /// <typeparam name="TRef">Type of entity class being looked up</typeparam>
        public virtual TRef RequiredEntityRef<TRef>(IUnitOfWork uow, int? id, int currentIndex, string fieldName)
            where TRef : class
        {
            return id.HasValue ? RequiredEntityRef<TRef>(uow, id.Value, currentIndex, nameof(id.Value)) : default(TRef);
        }

        #endregion
    }
}