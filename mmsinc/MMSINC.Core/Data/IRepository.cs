using System.Collections.Generic;

namespace MMSINC.Data
{
    public interface IBaseRepository
    {
        bool Exists(int id);
    }

    public interface IBaseRepository<TEntity> : IBaseRepository
    {
        #region Exposed Methods

        void Delete(TEntity entity);
        /// <summary>
        /// Cause <paramref name="entity"/> and/or any changes to it to be persisted immediately (as opposed
        /// to being marked for later saving via some flush or commit method, which this implementation does
        /// not support). 
        /// </summary>
        TEntity Save(TEntity entity);
        /// <summary>
        /// Cause each of the <paramref name="entities"/> and/or any changes to them to be persisted
        /// immediately (as opposed to being marked for later saving via some flush or commit method, which
        /// this implementation does not support).
        /// </summary>
        void Save(IEnumerable<TEntity> entities);

        void Update(TEntity entity);

        /// <summary>
        /// Performs a database lookup, returns null if TEntity with id does not exist.
        /// </summary>
        /// <param name="id">Id to search for.</param>
        TEntity Find(int id);

        int GetIdentifier(TEntity entity);

        #endregion
    }
}
