using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MMSINC.Common;

namespace MMSINC.Data.Linq
{
    public interface IRepository
    {
        #region Properties

        int CurrentIndex { get; }
        object SelectedDataKey { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Changes the currently focused entity to the entity with the
        /// specified index (retrieving said entity from persistence in
        /// the process).
        /// </summary>
        /// <param name="selectedIndex">
        /// Index of the entity to receive focus.
        /// </param>
        void RestoreFromPersistedState(int selectedIndex);

        void SetSelectedDataKey(object selectedDataKey);

        int Count();

        #endregion
    }

    public interface IRepository<TEntity> : IRepository
        where TEntity : class
    {
        #region Events

        /// <summary>
        /// To be called when the entity being focused on currently changes.
        /// </summary>
        event EventHandler<EntityEventArgs<TEntity>> CurrentEntityChanged,
                                                     EntityDeleted,
                                                     EntityInserted,
                                                     EntityUpdated;

        #endregion

        #region Properties

        /// <summary>
        /// Returns the currently focused entity object.
        /// </summary>
        TEntity CurrentEntity { get; }

        /// <summary>
        /// List of all the entity objects which the repository can
        /// retrieve.
        /// </summary>
        IList<TEntity> Entities { get; }

        Expression<Func<TEntity, bool>> FilterExpression { get; }

        string SortExpression { get; }
        //string SortDirection { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the currently focused entity with the values in
        /// the passed entity.
        /// </summary>
        /// <param name="entity">
        /// Entity object to derive the updated values from.
        /// </param>
        void UpdateCurrentEntity(TEntity entity);

        /// <summary>
        /// Updates the currently focused entity with the values in
        /// the passed entity. Null properties and associations 
        /// will overwrite the original values.
        /// </summary>
        /// <param name="entity">
        /// Entity object to derive the updated values from.
        /// </param>
        void UpdateCurrentEntityLiterally(TEntity entity);

        void InsertNewEntity(TEntity entity);

        void DeleteEntity(TEntity entity);

        TEntity Get(object id);

        IEnumerable<TEntity> GetFilteredSortedData(
            Expression<Func<TEntity, bool>> filterExpression,
            string sortExpression);

        int GetCountForExpression(
            Expression<Func<TEntity, bool>> filterExpression);

        void SetSelectedDataKeyForRPC(object selectedDataKey,
            Expression<Func<TEntity, bool>>
                filterExpression);

        #endregion
    }
}
