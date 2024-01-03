using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Common;
using MMSINC.Exceptions;
using MMSINC.Interface;
using IContainer = StructureMap.IContainer;

namespace MMSINC.Data.Linq
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        #region Constants

        private const short UNSET_INDEX = -1;
        private const string DATA_CONTEXT_KEY = "_dataContext";
        private const string SELECT_QUERY_FORMAT = "SELECT * FROM {0} WHERE {1} = {{0}}";

        private struct NecessaryTypes
        {
            public static readonly Type TableAttribute = typeof(TableAttribute);
            public static readonly Type ColumnAttribute = typeof(ColumnAttribute);
            public static readonly Type EntityType = typeof(TEntity);
        }

        #endregion

        #region Private Members

        private int _currentEntityIndex = UNSET_INDEX;
        private object _currentEntityDataKey;

        private Expression<Func<TEntity, bool>> _filterExpression;

        #endregion

        #region Private Static Members

        protected static ITable<TEntity> _dataTable;

        private static string _selectQuery,
                              _tableName,
                              _primaryKeyDBColumnName;

        private static PropertyInfo[] _columns;
        private static PropertyInfo _primaryKey;

        private static bool _sorted;
        private static List<TEntity> _sortedList;

        #endregion

        #region Properties

        public TEntity CurrentEntity
        {
            get
            {
                return (_currentEntityDataKey == null)
                    ? null
                    : GetEntity(SelectedDataKey);
            }
        }

        public IList<TEntity> Entities
        {
            get { return SelectAllAsList(); }
        }

        public int CurrentIndex
        {
            get { return _currentEntityIndex; }
        }

        public object SelectedDataKey
        {
            get { return _currentEntityDataKey; }
        }

        /// <summary>
        /// Adding this in to allow for the Entities Property to 
        /// Pull from a filtered set of entites.
        /// </summary>
        public Expression<Func<TEntity, bool>> FilterExpression
        {
            get
            {
                if (_filterExpression == null)
                    return PredicateBuilder.True<TEntity>();
                return _filterExpression;
            }
            protected set { _filterExpression = value; }
        }

        public string SortExpression { get; set; }

        #endregion

        #region Static Properties

        protected static IDataContext DataContext
        {
            get
            {
                if (HttpContext.Current == null)
                    throw new InvalidContextException(
                        "Cannot retrieve a DataContext when not running within a valid HttpContext.");
                var dc =
                    (IDataContext)
                    HttpContext.Current.Items[DATA_CONTEXT_KEY];
                if (dc == null)
                {
                    dc = DependencyResolver.Current.GetService<IContainer>().GetInstance<IDataContext>();
                    HttpContext.Current.Items.Add(DATA_CONTEXT_KEY,
                        dc);
                }

                return dc;
            }
        }

        protected static ITable<TEntity> DataTable
        {
            get
            {
                return _dataTable ?? new TableWrapper<TEntity>(
                    DataContext.GetTable<TEntity>());
            }
        }

        protected static string SelectQuery
        {
            get
            {
                if (_selectQuery == null)
                    _selectQuery = String.Format(SELECT_QUERY_FORMAT,
                        TableName, PrimaryKeyDBColumnName);
                return _selectQuery;
            }
        }

        protected static string TableName
        {
            get
            {
                if (_tableName == null)
                {
                    var att = NecessaryTypes.EntityType.GetCustomAttributes(NecessaryTypes.TableAttribute, false)
                                            .FirstOrDefault();
                    _tableName = (att == null) ? "" : ((TableAttribute)att).Name;
                }

                return _tableName;
            }
        }

        protected static PropertyInfo PrimaryKey
        {
            get
            {
                if (_primaryKey == null)
                    foreach (PropertyInfo prop in Columns)
                    foreach (ColumnAttribute col in prop.GetCustomAttributes(NecessaryTypes.ColumnAttribute, false))
                        if (col.IsPrimaryKey)
                            return (_primaryKey = prop);
                return _primaryKey;
            }
        }

        protected static string PrimaryKeyDBColumnName
        {
            get
            {
                if (_primaryKeyDBColumnName == null)
                    _primaryKeyDBColumnName = GetDBColumnName(PrimaryKey);
                return _primaryKeyDBColumnName;
            }
        }

        protected static PropertyInfo[] Columns
        {
            get
            {
                if (_columns == null)
                    _columns = (from p in NecessaryTypes.EntityType.GetProperties()
                                where p.GetIndexParameters().Length == 0 &&
                                      p.GetCustomAttributes(NecessaryTypes.ColumnAttribute, false).FirstOrDefault() !=
                                      null
                                select p).ToArray();
                return _columns;
            }
        }

        public static bool Sorted
        {
            get { return _sorted; }
        }

        public static List<TEntity> SortedList
        {
            get { return _sortedList; }
        }

        #endregion

        #region Events

        public event EventHandler<EntityEventArgs<TEntity>> CurrentEntityChanged,
                                                            EntityDeleted,
                                                            EntityInserted,
                                                            EntityUpdated;

        #endregion

        #region Event Passthroughs

        protected virtual void OnCurrentEntityChanged(TEntity entity)
        {
            if (CurrentEntityChanged != null)
                CurrentEntityChanged(this, new EntityEventArgs<TEntity>(entity));
        }

        protected virtual void OnEntityDeleted(TEntity entity)
        {
            if (EntityDeleted != null)
                EntityDeleted(this, new EntityEventArgs<TEntity>(entity));
        }

        protected virtual void OnEntityInserted(TEntity entity)
        {
            if (EntityInserted != null)
                EntityInserted(this, new EntityEventArgs<TEntity>(entity));
        }

        protected virtual void OnEntityUpdated(TEntity entity)
        {
            if (EntityUpdated != null)
                EntityUpdated(this, new EntityEventArgs<TEntity>(entity));
        }

        #endregion

        #region Private Static Methods

        private static void UpdateOriginalFromChanged(ref TEntity destination, TEntity source)
        {
            EntityMerger.Merge(ref destination, source);
        }

        private static void UpdateOriginalFromChangedLiterally(ref TEntity destination, TEntity source)
        {
            EntityMerger.Merge(ref destination, source, true);
        }

        private static string GetDBColumnName(PropertyInfo prop)
        {
            var att = prop.GetCustomAttributes(NecessaryTypes.ColumnAttribute, false).FirstOrDefault();
            return (att == null || ((ColumnAttribute)att).Name == null) ? prop.Name : ((ColumnAttribute)att).Name;
        }

        private static object GetPrimaryKeyValue(TEntity entity)
        {
            return PrimaryKey.GetValue(entity, null);
        }

        private static void SubmitChanges()
        {
            DataContext.SubmitChanges();
        }

        #endregion

        #region Private Instance Methods

        private void SetEntityIndex(int index)
        {
            _currentEntityIndex = index;
        }

        private void SetEntityDataKey(object key)
        {
            _currentEntityDataKey = key;
        }

        private void SetIndexFromDataKey(object selectedDataKey)
        {
            var entities = GetFilteredSortedData(FilterExpression, SortExpression).ToList();
            if (entities.Count > 0 && entities.Any(e => GetPrimaryKeyValue(e).ToString() == selectedDataKey.ToString()))
            {
                var entity =
                    entities.Where(e =>
                        GetPrimaryKeyValue(e).ToString() == selectedDataKey.ToString()).First();
                SetEntityIndex(entities.IndexOf(entity));
            }
        }

        #endregion

        #region Exposed Static Methods

        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void Update(TEntity entity)
        {
            var original = GetEntity(entity);
            UpdateOriginalFromChanged(ref original, entity);
            SubmitChanges();
        }

        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateLiterally(TEntity entity)
        {
            var original = GetEntity(entity);
            UpdateOriginalFromChangedLiterally(ref original, entity);
            SubmitChanges();
        }

        //TODO: MyAssert.Throws doesn't return the correct exception when the simulator is not present.
        [DataObjectMethod(DataObjectMethodType.Insert)]
        public static void Insert(TEntity entity)
        {
            DataTable.InsertOnSubmit(entity);
            SubmitChanges();
        }

        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void Delete(TEntity entity)
        {
            DataTable.DeleteOnSubmit(GetEntity(entity));
            SubmitChanges();
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static IList<TEntity> SelectAllAsList()
        {
            return SelectAllAsList(string.Empty);
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static IList<TEntity> SelectAllAsList(string sortExpression)
        {
            var list = DataTable.ToList();
            if (!String.IsNullOrEmpty(sortExpression))
                list.Sort(new EntityComparer<TEntity>(sortExpression));
            return list;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static TEntity GetEntity(TEntity entity)
        {
            return GetEntity(GetPrimaryKeyValue(entity));
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static TEntity GetEntity(object id)
        {
            var returnValue = DataContext.ExecuteQuery<TEntity>(String.Format(SelectQuery, id)).FirstOrDefault();
            return returnValue;
        }

        #endregion

        #region Exposed Instance Methods

        public TEntity Get(object id)
        {
            return GetEntity(id);
        }

        public void RestoreFromPersistedState(int selectedEntityIndex)
        {
            // this has been done already, we don't need to do it again.
            if (_currentEntityIndex == selectedEntityIndex) return;

            //SetEntityIndex(selectedEntityIndex);
            SetEntityDataKey(GetPrimaryKeyValue(Entities[selectedEntityIndex]));
            OnCurrentEntityChanged(CurrentEntity);
        }

        public void RestoreFromPersistedStateInsert(TEntity entity)
        {
            SetEntityDataKey(GetPrimaryKeyValue(entity));
            OnCurrentEntityChanged(entity);
        }

        public void SetSelectedDataKeyForRPC(object selectedDataKey, Expression<Func<TEntity, bool>> filterExpression)
        {
            FilterExpression = filterExpression;
            SetSelectedDataKey(selectedDataKey);
        }

        public void SetSelectedDataKey(object selectedDataKey)
        {
            if (_currentEntityDataKey == selectedDataKey) return;

            SetEntityDataKey(selectedDataKey);
            SetIndexFromDataKey(selectedDataKey);
            if (CurrentEntity != null)
                OnCurrentEntityChanged(CurrentEntity);
        }

        public virtual void UpdateCurrentEntity(TEntity entity)
        {
            Update(entity);
            OnEntityUpdated(entity);
        }

        public virtual void UpdateCurrentEntityLiterally(TEntity entity)
        {
            UpdateLiterally(entity);
            OnEntityUpdated(entity);
        }

        public virtual void InsertNewEntity(TEntity entity)
        {
            Insert(entity);
            OnEntityInserted(entity);
            //RestoreFromPersistedState(Entities.IndexOf(entity));
            RestoreFromPersistedStateInsert(entity);
        }

        public virtual void DeleteEntity(TEntity entity)
        {
            Delete(entity);
            OnEntityDeleted(entity);
        }

        public int Count()
        {
            return DataTable.Count();
        }

        public IEnumerable<TEntity> GetFilteredSortedData(Expression<Func<TEntity, bool>> filterExpression,
            string sortExpression)
        {
            FilterExpression = filterExpression;
            SortExpression = sortExpression;

            var source = GetFilteredData(filterExpression);

            return !String.IsNullOrEmpty(sortExpression)
                ? source.Sorting().Sort<TEntity>(sortExpression)
                : source;
        }

        protected virtual IEnumerable<TEntity> GetFilteredData(Expression<Func<TEntity, bool>> filterExpression)
        {
            return DataTable.Where(filterExpression);
        }

        public int GetCountForExpression(Expression<Func<TEntity, bool>> filterExpression)
        {
            return DataTable.Count(filterExpression);
        }

        #endregion
    }
}
